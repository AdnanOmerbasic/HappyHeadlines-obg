using EasyNetQ;
using Events;
using NewsletterService.Application.Interfaces;

namespace NewsletterService.Api.Messaging
{
    public class NewsletterSubscriptionWelcomeMailQueues: BackgroundService
    {
        private readonly IBus _bus;
        private readonly IServiceProvider _sp;
        private readonly ILogger<NewsletterSubscriptionWelcomeMailQueues> _logger;

        public NewsletterSubscriptionWelcomeMailQueues(IBus bus, IServiceProvider sp, ILogger<NewsletterSubscriptionWelcomeMailQueues> logger)
        {
            _bus = bus;
            _sp = sp;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting subscription for  CreateSubscriptionEvent");
            return _bus.PubSub.SubscribeAsync<CreateSubscriptionEvent>("newsletter-welcome-mail", async evt =>
            {
                _logger.LogInformation("Received CreateSubscriptionEvent for Email: {Email}", evt.Email);
                using var scope = _sp.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<IEmailSenderClient>();
                await sender.SendWelcomeEmailAsync(evt.Email, stoppingToken);

                _logger.LogInformation($"Welcome email sent to {evt.Email}");
            });
        }

    }
}
