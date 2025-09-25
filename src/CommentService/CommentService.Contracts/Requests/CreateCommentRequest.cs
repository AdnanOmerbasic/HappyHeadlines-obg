using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentService.Contracts.Requests
{
    public class CreateCommentRequest
    {
        public required string Content { get; set; }
    }
}
