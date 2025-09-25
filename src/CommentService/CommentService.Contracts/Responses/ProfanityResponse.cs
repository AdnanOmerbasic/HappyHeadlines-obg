using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentService.Contracts.Responses
{
    public class ProfanityResponse
    {
        public required bool IsContentClean { get; set; }
        public required string FilteredText { get; set; } = string.Empty;
    }
}
