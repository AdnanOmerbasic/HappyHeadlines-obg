using DraftService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftService.Application.Interfaces
{
    public interface IDraftRepository
    {
        Task<Draft> CreateDraftAsync(Draft draft, CancellationToken ct);
        Task<IEnumerable<Draft>> GetAllDraftAsync(CancellationToken ct);
        Task<Draft?> GetDraftByIdAsync(int id, CancellationToken ct);
        Task<Draft?> UpdateDraftAsync(Draft draft, CancellationToken ct);
        Task<Draft?> DeleteDraftAsync(int id, CancellationToken ct);
    }
}
