using BlogAppp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogAppp.IServices
{
    public interface IBlogService
    {
        Task<List<Blog>> GetAllBlogsAsync(int? categoryId);
        Task<Blog?> GetBlogByIdAsync(int id);
        Task CreateBlogAsync(Blog blog);
        Task UpdateBlogAsync(Blog blog);
        Task DeleteBlogAsync(int id);
    }
}
