using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleService.Domain.Entity
{
    public enum Continent
    {
        AF,
        AN,
        AS,
        EU,
        NA,
        AU,
        SA,
        GL
    }

    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public Continent Continent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Article()
        {

        }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(Title)}={Title}, {nameof(Content)}={Content}, {nameof(Continent)}={Continent.ToString()}, {nameof(CreatedAt)}={CreatedAt.ToString()}, {nameof(UpdatedAt)}={UpdatedAt.ToString()}}}";
        }
    }
}
