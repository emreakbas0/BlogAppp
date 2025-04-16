using BlogAppp.Models;
using System.Threading.Tasks;

namespace BlogAppp.IServices
{
    public interface ICommentService
    {
        Task CreateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int id);
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<List<Comment>> GetCommentsByBlogIdAsync(int blogId);
    }
}
