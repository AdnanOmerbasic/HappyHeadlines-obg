using EasyNetQ;
using Events;
using Microsoft.AspNetCore.Mvc;
using Monitoring.Shared;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsletterService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class newslettersController : ControllerBase
    {
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
