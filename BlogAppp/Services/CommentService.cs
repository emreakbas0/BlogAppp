using System.Collections.Generic;
using System.Threading.Tasks;
using BlogAppp.Models;
using BlogAppp.IServices;
using BlogAppp.IRepositories;

namespace BlogAppp.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Comment>> GetCommentsByBlogIdAsync(int blogId)
        {
            return await _unitOfWork.Comments.Where(c => c.BlogId == blogId);
        }

        public async Task CreateCommentAsync(Comment comment)
        {
            await _unitOfWork.Comments.AddAsync(comment);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(id);
            if (comment != null)
            {
                _unitOfWork.Comments.Remove(comment);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _unitOfWork.Comments.GetByIdAsync(id);
        }
    }
}
