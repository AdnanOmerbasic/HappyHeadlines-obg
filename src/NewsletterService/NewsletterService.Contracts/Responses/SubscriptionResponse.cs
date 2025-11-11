using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsletterService.Contracts.Responses
{
    public class SubscriptionResponse
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required bool IsSubscribed { get; set; }
    }
}
