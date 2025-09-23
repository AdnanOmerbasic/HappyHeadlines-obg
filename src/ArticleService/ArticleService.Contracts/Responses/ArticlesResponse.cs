using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleService.Contracts.Responses
{
    public class ArticlesResponse
    {
        public required IEnumerable<ArticleResponse> Articles { get; set; } = Enumerable.Empty<ArticleResponse>();
    }
}
