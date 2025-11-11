using Microsoft.EntityFrameworkCore;
using SubscriberService.Application.Db;
using SubscriberService.Application.Interface;
using SubscriberService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriberService.Application.Repository
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly SubscriberDbContext _dbContext;
        public SubscriberRepository(SubscriberDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Subscription?> CancelSubscriptionAsync(int id, CancellationToken ct)
        {
            var existingSubscription = await _dbContext.Subscriptions.FirstOrDefaultAsync(s => s.Id == id, ct);
            if(existingSubscription == null || !existingSubscription.IsSubscribed)
            {
                return null;
            }
            existingSubscription.IsSubscribed = false;
            existingSubscription.UpdatedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(ct);
            return existingSubscription;
        }

        public async Task<Subscription?> CreateSubscriptionAsync(Subscription subscription, CancellationToken ct)
        {
            var existingSubscription = await _dbContext.Subscriptions.FirstOrDefaultAsync(s => s.Email == subscription.Email, ct);
            if(existingSubscription != null && existingSubscription.IsSubscribed)
            {
                return null;
            }
            if(existingSubscription != null && !existingSubscription.IsSubscribed)
            {
                existingSubscription.IsSubscribed = true;
                existingSubscription.UpdatedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync(ct);
                return existingSubscription;
            }
            _dbContext.Add(subscription);
            await _dbContext.SaveChangesAsync(ct);
            return subscription;
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync(CancellationToken ct)
        {
            return await _dbContext.Subscriptions.AsNoTracking().ToListAsync(ct);
        }

        public async Task<Subscription?> GetSubscriptionByEmailAsync(string email, CancellationToken ct)
        {
            var existingSubscription = await _dbContext.Subscriptions.FirstOrDefaultAsync(s => s.Email == email, ct);
            if(existingSubscription == null)
            {
                return null;
            }
            return existingSubscription;
        }

        public async Task<Subscription?> GetSubscriptionByIdAsync(int id, CancellationToken ct)
        {
            var existingSubscription = await _dbContext.Subscriptions.FirstOrDefaultAsync(s => s.Id == id, ct);
            if(existingSubscription == null)
            {
                return null;
            }
            return existingSubscription;
        }
    }
}
