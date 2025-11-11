using SubscriberService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriberService.Application.Interface
{
    public interface ISubscriberRepository
    {
        Task<Subscription?> CreateSubscriptionAsync(Subscription subscription, CancellationToken ct);
        Task<Subscription?> CancelSubscriptionAsync(int id, CancellationToken ct);
        Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync(CancellationToken ct);
        Task<Subscription?> GetSubscriptionByIdAsync(int id, CancellationToken ct);
        Task<Subscription?> GetSubscriptionByEmailAsync(string email, CancellationToken ct);
    }
}
