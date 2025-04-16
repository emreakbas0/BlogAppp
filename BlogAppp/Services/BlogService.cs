using System.Collections.Generic;
using System.Threading.Tasks;
using BlogAppp.Models;
using BlogAppp.IRepositories;
using BlogAppp.IServices;
using BlogAppp.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogAppp.Services
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public BlogService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Blog>> GetAllBlogsAsync(int? categoryId)
        {
            var query = _context.Blogs
                .Include(b => b.AppUser)     
                .Include(b => b.Category)   
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Blog?> GetBlogByIdAsync(int id)
        {
            return await _context.Blogs
                .Include(b => b.AppUser)   
                .Include(b => b.Category)
                .Include(b => b.Comments)
                    .ThenInclude(c => c.AppUser) 
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task CreateBlogAsync(Blog blog)
        {
            await _unitOfWork.Blogs.AddAsync(blog);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateBlogAsync(Blog blog)
        {
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteBlogAsync(int id)
        {
            var blog = await _unitOfWork.Blogs.GetByIdAsync(id);
            if (blog != null)
            {
                _unitOfWork.Blogs.Remove(blog);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}