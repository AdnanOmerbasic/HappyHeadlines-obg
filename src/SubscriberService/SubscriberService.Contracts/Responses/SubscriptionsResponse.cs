using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriberService.Contracts.Responses
{
    public class SubscriptionsResponse
    {
        public IEnumerable<SubscriptionResponse> Subscriptions { get; set; } = Enumerable.Empty<SubscriptionResponse>();
    }
}
