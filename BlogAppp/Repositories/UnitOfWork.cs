using BlogAppp.IRepositories;
using BlogAppp.Data;
using BlogAppp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace BlogAppp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGenericRepository<Blog> Blogs { get; private set; }
        public IGenericRepository<Category> Categories { get; private set; }
        public IGenericRepository<Comment> Comments { get; private set; }
        public IGenericRepository<AppUser> Users { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Blogs = new GenericRepository<Blog>(_context);
            Categories = new GenericRepository<Category>(_context);
            Comments = new GenericRepository<Comment>(_context);
             Users = new GenericRepository<AppUser>(_context); 
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}