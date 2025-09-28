using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentService.Domain.Entity
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
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Continent Continent { get; set; }
        public int ArticleId { get; set; }

        public Comment()
        {

        }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(Content)}={Content}, {nameof(CreatedAt)}={CreatedAt.ToString()}, {nameof(UpdatedAt)}={UpdatedAt.ToString()}, {nameof(Continent)}={Continent.ToString()}, {nameof(ArticleId)}={ArticleId.ToString()}}}";
        }
    }
}
