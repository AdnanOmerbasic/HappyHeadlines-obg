using ProfanityService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfanityService.Application.Interface
{
    public interface IProfanityRepository
    {
        Task<IEnumerable<Profanity>> GetAllAsync();
        Task<(string, bool)> CheckForProfanities(Profanity profanity, CancellationToken ct);
    }
}
