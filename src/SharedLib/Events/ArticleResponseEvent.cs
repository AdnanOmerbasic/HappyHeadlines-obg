using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public record ArticleDto(string Title, string Content, string AuthorName, Continent Continent);
    public class ArticleResponseEvent
    {
        public List<ArticleDto> Articles { get; set; } = new();
    }
}
