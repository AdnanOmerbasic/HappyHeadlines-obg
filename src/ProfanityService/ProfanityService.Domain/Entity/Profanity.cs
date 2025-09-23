using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfanityService.Domain.Entity
{
    public class Profanity
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public Profanity()
        {
        }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(Word)}={Word}}}";
        }
    }
}
