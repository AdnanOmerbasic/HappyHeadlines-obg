using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriberService.Domain.Entity
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsSubscribed { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Subscription() { }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(Email)}={Email}, {nameof(IsSubscribed)}={IsSubscribed.ToString()}, {nameof(CreatedAt)}={CreatedAt.ToString()}, {nameof(UpdatedAt)}={UpdatedAt.ToString()}}}";
        }
    }
}
