using ArticleService.Application.Interfaces;
using ArticleService.Domain.Entity;
using EasyNetQ;
using Events;
using Monitoring.Shared;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;
using System.Linq;
using DomainContinent = ArticleService.Domain.Entity.Continent;
using EventContinent = Events.Continent;

namespace ArticleService.Api.Messaging
{
    public class ArticlesRespondRequests: BackgroundService
    {
        private readonly IBus _bus;
        private readonly IServiceProvider _sp;
        private static readonly TextMapPropagator Prop = new TraceContextPropagator();
        private static DomainContinent ToDomain(EventContinent c) => (DomainContinent)c;
        private static EventContinent ToEvent(DomainContinent c) => (EventContinent)c;
        public ArticlesRespondRequests(IBus bus, IServiceProvider sp) { _bus = bus; _sp = sp; }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _ = _bus.Rpc.RespondAsync<ArticleRequestEvent, ArticleResponseEvent>(async req =>
            {
                var parent = Prop.Extract(default, req.Headers, (h, k) =>
                    req.Headers.TryGetValue(k, out var v) ? new[] { v } : Array.Empty<string>());

                using var span = MonitoringService.ActivitySource
                    .StartActivity("rpc GetArticles", ActivityKind.Consumer, parent.ActivityContext);

                using var scope = _sp.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IArticleRepository>();

                var items = await repo.GetAllAsync(ToDomain(req.Continent), stoppingToken);

                span?.SetStatus(ActivityStatusCode.Ok);
                return new ArticleResponseEvent
                {
                    Articles = items.Select(a => new ArticleDto(a.Title, a.Content, a.AuthorName, ToEvent(a.Continent))).ToList()
                };
            });

            return Task.CompletedTask;
        }
    }
}
