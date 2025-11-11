using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriberService.Contracts.Requests
{
    public class CreateSubscriptionRequest
    {
        public required string Email { get; set; }
    }
}
