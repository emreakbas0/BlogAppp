using BlogAppp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogAppp.IServices
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category category);
    }
}
