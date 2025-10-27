using ArticleService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleService.Application.Interfaces
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetAllAsync(Continent continent, CancellationToken ct);
        Task<Article> CreateArticleAsync(Article article, CancellationToken ct);
        Task<Article?> UpdateArticleAsync(Article article, CancellationToken ct);
        Task<Article?> DeleteArticleAsync(int id, Continent continent, CancellationToken ct);
        Task<Article?> GetArticleByIdAsync(int id, Continent continent, CancellationToken ct);
        Task<IEnumerable<Article>> GetRecentArticles(int days, Continent continent, CancellationToken ct);
    }
}
