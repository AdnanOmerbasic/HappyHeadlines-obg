using EasyNetQ;
using Events;
using Monitoring.Shared;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;

namespace NewsletterService.Api.Messaging
{
    public class NewsletterSubscribeQueues: BackgroundService
    {
        private readonly IBus _bus;
        public NewsletterSubscribeQueues(IBus bus)
        {
            _bus = bus;
        }
        protected override Task ExecuteAsync(CancellationToken ct)
        {
            _ = _bus.PubSub.SubscribeAsync<CreateArticleEvent>("newsletter-request", async evt =>
            {
                var parent = new TraceContextPropagator().Extract(default, evt.Headers, (h, k) =>
                    h.TryGetValue(k, out var v) ? new[] { v } : Array.Empty<string>());
                using var span = MonitoringService.ActivitySource.StartActivity(
                    "consume CreateArticleEvent - newsletter", ActivityKind.Consumer, parent.ActivityContext);

                await Task.CompletedTask;
            }, cfg => cfg.WithTopic("#"));

            return Task.CompletedTask;
        }
    }
}
