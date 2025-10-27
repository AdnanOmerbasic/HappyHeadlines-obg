using ArticleService.Application.Interfaces;
using ArticleService.Contracts.Requests;
using ArticleService.Domain.Entity;
using EasyNetQ;
using Monitoring.Shared;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Events;
using Microsoft.Extensions.Hosting;

namespace ArticleService.Api.Messaging
{
    public class ArticleSubscribeQueues:BackgroundService
    {
        private readonly IBus _bus;
        private readonly IServiceProvider _sp;
        private static readonly TextMapPropagator Prop = new TraceContextPropagator();
        private static ArticleService.Domain.Entity.Continent MapContinent(Events.Continent c)
    => (ArticleService.Domain.Entity.Continent)c;

        public ArticleSubscribeQueues(IBus bus, IServiceProvider sp) { _bus = bus; _sp = sp; }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _ = _bus.PubSub.SubscribeAsync<CreateArticleEvent>(
          subscriptionId: "article-db",
          onMessage: async evt =>
          {
              var parent = Prop.Extract(default, evt.Headers, (h, k) =>
                  h.TryGetValue(k, out var v) ? new[] { v } : Array.Empty<string>());

              Baggage.Current = parent.Baggage;

              using var span = MonitoringService.ActivitySource
                  .StartActivity("consume CreateArticleEvent", ActivityKind.Consumer, parent.ActivityContext);

              using var scope = _sp.CreateScope();
              var repo = scope.ServiceProvider.GetRequiredService<IArticleRepository>();

              await repo.CreateArticleAsync(new Article
              {
                  Title = evt.Title,
                  Content = evt.Content,
                  AuthorName = evt.AuthorName,
                  Continent = MapContinent(evt.Continent),
              }, stoppingToken);

              span?.SetStatus(ActivityStatusCode.Ok);
          },
          cfg => cfg.WithTopic("#"));

            return Task.CompletedTask;
        }
    }
}
