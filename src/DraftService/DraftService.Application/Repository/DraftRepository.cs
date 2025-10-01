using DraftService.Application.Db;
using DraftService.Application.Interfaces;
using DraftService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftService.Application.Repository
{
    public class DraftRepository : IDraftRepository
    {
        private readonly DraftDbContext _dbContext;

        public DraftRepository(DraftDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Draft> CreateDraftAsync(Draft draft, CancellationToken ct)
        {
                _dbContext.Add(draft);
                await _dbContext.SaveChangesAsync(ct);
                return draft;
        }

        public async Task<Draft?> DeleteDraftAsync(int id, CancellationToken ct)
        {
            var existingDraft = await _dbContext.Drafts.FirstOrDefaultAsync(a => a.Id == id, ct);
            if (existingDraft == null)
            {
                return null;
            }
            _dbContext.Remove(existingDraft);
            await _dbContext.SaveChangesAsync(ct);
            return existingDraft;
        }

        public async Task<IEnumerable<Draft>> GetAllDraftAsync(CancellationToken ct)
        {
            return await _dbContext.Drafts.AsNoTracking().ToListAsync(ct);
        }

        public async Task<Draft?> GetDraftByIdAsync(int id, CancellationToken ct)
        {
            var existingDraft =  await _dbContext.Drafts.FirstOrDefaultAsync(d => d.Id == id, ct);
            if (existingDraft == null)
            {
                return null;
            }
            return existingDraft;
        }

        public async Task<Draft?> UpdateDraftAsync(Draft draft, CancellationToken ct)
        {
            var existingDraft =  await _dbContext.Drafts.FirstOrDefaultAsync(a => a.Id == draft.Id, ct);
            if (existingDraft == null)
            {
                return null;
            }
            existingDraft.Title = draft.Title;
            existingDraft.Content = draft.Content;
            existingDraft.AuthorName = draft.AuthorName;
            existingDraft.Continent = draft.Continent;
            existingDraft.UpdatedAt = DateTime.UtcNow;

            _dbContext.Update(existingDraft);
            await _dbContext.SaveChangesAsync(ct);
            return existingDraft;
        }
    }
}
