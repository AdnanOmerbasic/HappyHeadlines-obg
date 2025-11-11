using EasyNetQ;
using Events;
using Microsoft.AspNetCore.Mvc;
using Monitoring.Shared;
using NewsletterService.Contracts.Responses;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Polly;
using Polly.CircuitBreaker;


namespace NewsletterService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class newslettersController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClient;

        private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy =
            Policy<HttpResponseMessage>.Handle<HttpRequestException>()
            .OrResult(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));

        public newslettersController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        [Route("subscribers")]
        public async Task<IActionResult> GetSubscribers(CancellationToken ct)
        {
            var client = _httpClient.CreateClient("SubscriberService");
            try
            {
                var response = await _circuitBreakerPolicy.ExecuteAsync((token) => client.GetAsync("api/subscriptions", token), ct);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Failed to retrieve subscribers");
                }
                var subscribers = await response.Content.ReadFromJsonAsync<SubscriptionsResponse>(ct);
                return Ok(subscribers);
            }
            catch (BrokenCircuitException)
            {
                return StatusCode(503, "Subscriber service is currently unavailable. Please try again later.");
            }
        }

        [HttpPost]
        [Route("{continent}/daily")]
        public async Task<IActionResult> DailyNewsletter([FromRoute] Continent continent, [FromServices] IBus bus, CancellationToken ct)
        {
            using var activity = MonitoringService.ActivitySource.StartActivity("Publish DailyNewsletterEvent");
            activity?.SetTag("continent", continent.ToString());

            var req = new ArticleRequestEvent
            {
                Continent = continent,
                Headers = new()
            };

            var propagator = new TraceContextPropagator();
            var context = new PropagationContext(activity?.Context ?? default, Baggage.Current);
            propagator.Inject(context, req.Headers, (carrier, key, value) =>
            {
                carrier[key] = value;
            });

            var res = await bus.Rpc.RequestAsync<ArticleRequestEvent, ArticleResponseEvent>(req, ct);

            if (res == null)
            {
                return NoContent();
            }

            return Ok(res);
        }

    }
}
