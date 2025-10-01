using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftService.Domain.Entity
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
    public class Draft
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public Continent Continent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Draft()
        {

        }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(Title)}={Title}, {nameof(Content)}={Content}, {nameof(AuthorName)}={AuthorName}, {nameof(Continent)}={Continent.ToString()}, {nameof(CreatedAt)}={CreatedAt.ToString()}, {nameof(UpdatedAt)}={UpdatedAt.ToString()}}}";
        }
    }
}
