using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftService.Contracts.Responses
{
    public class DraftsResponse
    {
        public IEnumerable<DraftResponse> Drafts { get; set; } = Enumerable.Empty<DraftResponse>();
    }
}
