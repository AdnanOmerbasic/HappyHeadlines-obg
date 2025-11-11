using EasyNetQ;
using Events;
using Microsoft.AspNetCore.Mvc;
using SubscriberService.Application.Interface;
using SubscriberService.Contracts.Requests;
using SubscriberService.Contracts.Responses;
using SubscriberService.Domain.Entity;


namespace SubscriberService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class subscriptionsController : ControllerBase
    {
        private readonly ISubscriberRepository _repo;
        public subscriptionsController(ISubscriberRepository repo)
        {
            _repo = repo;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubscriptionRequest req, [FromServices] IBus bus, CancellationToken ct)
        {
            var subscription = new Subscription
            {
                Email = req.Email,
                IsSubscribed = true,
            };
            var createdSubscription = await _repo.CreateSubscriptionAsync(subscription, ct);
            if(createdSubscription == null)
            {
                return BadRequest("Subscription could not be created.");
            }

            await bus.PubSub.PublishAsync(new CreateSubscriptionEvent
            {
                Email = createdSubscription.Email,
            }, ct);

            return CreatedAtAction(nameof(Get), new { id = createdSubscription.Id }, createdSubscription);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken ct)
        {
            var getSubscription = await _repo.GetSubscriptionByIdAsync(id, ct);
            if(getSubscription == null)
            {
                return NotFound();
            }
            return Ok(getSubscription);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var subscriptions = await _repo.GetAllSubscriptionsAsync(ct);
            var response = new SubscriptionsResponse
            {
                Subscriptions = subscriptions.Select(s => new SubscriptionResponse
                {
                    Id = s.Id,
                    Email = s.Email,
                    IsSubscribed = s.IsSubscribed,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("email")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email, CancellationToken ct)
        {
            var subscription = await _repo.GetSubscriptionByEmailAsync(email, ct);
            if(subscription == null)
            {
                return NotFound();
            }
            var response = new SubscriptionResponse
            {
                Id = subscription.Id,
                Email = subscription.Email,
                IsSubscribed = subscription.IsSubscribed,
                CreatedAt = subscription.CreatedAt,
                UpdatedAt = subscription.UpdatedAt
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CancelSubscriptionRequest req, CancellationToken ct)
        {
            var updated = await _repo.CancelSubscriptionAsync(id, ct);
            if (updated == null)
            {
                return NotFound();
            }

            var response = new SubscriptionResponse
            {
                Id = updated.Id,
                Email = updated.Email,
                IsSubscribed = updated.IsSubscribed,
                UpdatedAt = updated.UpdatedAt
            };
            return Ok(response);
        }
    }
}
