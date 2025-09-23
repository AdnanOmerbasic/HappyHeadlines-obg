using CommentService.Application.Db;
using CommentService.Application.Interface;
using CommentService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentService.Application.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CommentDbContext _dbContext;

        public CommentRepository(CommentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Comment> CreateCommentAsync(Comment comment, CancellationToken ct)
        {
            _dbContext.Add(comment);
            await _dbContext.SaveChangesAsync(ct);
            return comment;

        }

        public async Task<Comment?> DeleteCommentAsync(int id, CancellationToken ct)
        {
            var existingComment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id, ct);
            if (existingComment == null)
            {
                return null;
            }
            _dbContext.Remove(existingComment);
            await _dbContext.SaveChangesAsync(ct);
            return existingComment;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync(Continent continent, int articleId, CancellationToken ct)
        {
            return await _dbContext.Comments.AsNoTracking().Where(c => c.Continent == continent && c.ArticleId == articleId).OrderBy(c => c.CreatedAt).ToListAsync(ct);
        }

        public async Task<Comment?> UpdateCommentAsync(Comment comment, CancellationToken ct)
        {
            var existingComment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == comment.Id, ct);
            if (existingComment == null)
            {
                return null;
            }
            existingComment.Content = comment.Content;
            existingComment.UpdatedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(ct);
            return existingComment;
        }
    }
}
