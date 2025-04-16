using System.Threading.Tasks;
using BlogAppp.IRepositories;
using BlogAppp.Models;

namespace BlogAppp.IRepositories
{
    public interface IUnitOfWork
    {
        IGenericRepository<Blog> Blogs { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Comment> Comments { get; }
        IGenericRepository<AppUser> Users { get; }

        Task<int> SaveAsync();
    }
}