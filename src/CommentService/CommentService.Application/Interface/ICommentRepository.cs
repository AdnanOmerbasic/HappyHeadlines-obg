using CommentService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentService.Application.Interface
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync(Continent continent, int articleId, CancellationToken ct);
        Task<Comment> CreateCommentAsync(Comment comment, CancellationToken ct);
        Task<Comment?> UpdateCommentAsync(Comment comment, CancellationToken ct);
        Task<Comment?> DeleteCommentAsync(int id, CancellationToken ct);
    }
}
