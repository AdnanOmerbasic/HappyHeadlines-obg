using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentService.Contracts.Requests
{
    public class UpdateCommentRequest
    {
        public required string Content { get; set; }
    }
}
