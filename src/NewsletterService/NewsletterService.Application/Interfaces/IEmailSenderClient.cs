using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsletterService.Application.Interfaces
{
    public interface IEmailSenderClient
    {
        Task SendWelcomeEmailAsync(string email, CancellationToken ct);
    }
}
