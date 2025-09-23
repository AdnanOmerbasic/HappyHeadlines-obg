using Microsoft.EntityFrameworkCore;
using ProfanityService.Application.Db;
using ProfanityService.Application.Interface;
using ProfanityService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProfanityService.Application.Repository
{
    public class ProfanityRepository : IProfanityRepository
    {
        private readonly ProfanityDbContext _dbContext;
        public ProfanityRepository(ProfanityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<(string, bool)> CheckForProfanities(Profanity profanity, CancellationToken ct)
        {

            string content = profanity.Word;
            var word = await _dbContext.Profanities.ToListAsync(ct);
            var foundWords = word.Where(w => profanity.Word.Contains(w.Word, StringComparison.OrdinalIgnoreCase));

            foreach (var w in foundWords)
            {
                var pattern = $@"\b{Regex.Escape(w.Word)}\b";
                var replacement = new string('*', w.Word.Length);
                content = Regex.Replace(content, pattern, replacement, RegexOptions.IgnoreCase);
            }

            return (content, foundWords.Any());
        }

        public async Task<IEnumerable<Profanity>> GetAllAsync()
        {
            return await _dbContext.Profanities.AsNoTracking().ToListAsync();
        }
    }
}
