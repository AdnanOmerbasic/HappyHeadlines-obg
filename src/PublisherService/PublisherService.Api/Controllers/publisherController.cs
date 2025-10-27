using EasyNetQ;
using EasyNetQ.Topology;
using Events;
using Microsoft.AspNetCore.Mvc;
using Monitoring.Shared;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PublisherService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class publisherController : ControllerBase
    {

        private static readonly TextMapPropagator Prop = new TraceContextPropagator();

        [HttpPost("publish")]
        public async Task<IActionResult> Publish([FromBody] CreateArticleEvent req,
                                                 [FromServices] IBus bus,
                                                 CancellationToken ct)
        {
            using var activity = MonitoringService.ActivitySource
                .StartActivity("publish CreateArticleEvent", ActivityKind.Producer);

            var propagator = new TraceContextPropagator();
            var context = new PropagationContext(activity?.Context ?? default, Baggage.Current);
            propagator.Inject(context, req.Headers, (carrier, key, value) =>
            {
                carrier[key] = value;
            });

            await bus.PubSub.PublishAsync(req, ct);

            return Ok("Published");
        }
    }
}
