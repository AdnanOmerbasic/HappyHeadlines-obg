using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class ArticleRequestEvent
    {
        public Continent Continent { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new();    
    }
}
