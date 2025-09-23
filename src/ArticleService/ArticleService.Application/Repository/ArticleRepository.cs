using ArticleService.Application.Interfaces;
using ArticleService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleService.Application.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IDbContextFactory _dbContextFactory;
        public ArticleRepository(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task<Article> CreateArticleAsync(Article article, CancellationToken ct)
        {
            await using var db = _dbContextFactory.CreateDbContext(article.Continent);
            db.Add(article);
            await db.SaveChangesAsync(ct);
            return article;
        }

        public async Task<Article?> DeleteArticleAsync(int id, Continent continent, CancellationToken ct)
        {
            await using var db = _dbContextFactory.CreateDbContext(continent);
            var existingArticle = await db.Articles.FirstOrDefaultAsync(a => a.Id == id, ct);
            if (existingArticle == null)
            {
                return null;
            }
            db.Remove(existingArticle);
            await db.SaveChangesAsync(ct);
            return existingArticle;
        }

        public async Task<IEnumerable<Article>> GetAllAsync(Continent continent, CancellationToken ct)
        {
            await using var db = _dbContextFactory.CreateDbContext(continent);
            return await db.Articles.AsNoTracking().ToListAsync(ct);
        }

        public async Task<Article?> GetArticleByIdAsync(int id, Continent continent, CancellationToken ct)
        {
            await using var db = _dbContextFactory.CreateDbContext(continent);
            var existingArticle = await db.Articles.FirstOrDefaultAsync(a => a.Id == id, ct);
            if (existingArticle == null)
            {
                return null;
            }
            return existingArticle;
        }

        public async Task<Article?> UpdateArticleAsync(Article article, CancellationToken ct)
        {
            await using var db = _dbContextFactory.CreateDbContext(article.Continent);
            var existingArticle = await db.Articles.FirstOrDefaultAsync(a => a.Id == article.Id, ct);
            if (existingArticle == null)
            {
                return null;
            }
            if (existingArticle.Continent != article.Continent)
            {
                db.Remove(existingArticle);
                await db.SaveChangesAsync(ct);

                await using var newDb = _dbContextFactory.CreateDbContext(article.Continent);
                newDb.Add(article);
                await newDb.SaveChangesAsync(ct);
                return article;
            }

            existingArticle.Title = article.Title;
            existingArticle.AuthorName = article.AuthorName;
            existingArticle.Content = article.Content;
            existingArticle.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync(ct);
            return existingArticle;

        }
    }
}
