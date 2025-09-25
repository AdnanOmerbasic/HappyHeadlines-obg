using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentService.Contracts.Responses
{
    public class CommentsResponse
    {
        public required IEnumerable<CommentResponse> Comments { get; set; } = Enumerable.Empty<CommentResponse>();
    }
}
