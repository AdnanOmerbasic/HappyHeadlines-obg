using ArticleService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleService.Contracts.Requests
{
    public class CreateArticleRequest
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string AuthorName { get; set; }
        public required Continent Continent { get; set; }

    }
}
