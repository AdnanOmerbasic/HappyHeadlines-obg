using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfanityService.Contracts.Requests
{
    public class CheckProfanityRequest
    {
        public required string Text { get; set; } = string.Empty;
    }
}
