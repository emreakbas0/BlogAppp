using BlogAppp.IRepositories;
using BlogAppp.IServices;
using BlogAppp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAppp.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IUnitOfWork _unitOfWork;

        public CommentController(ICommentService commentService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(Comment comment)
        {
            comment.CreatedAt = DateTime.Now;
            comment.AppUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

            ModelState.Remove(nameof(comment.AppUserId));
            ModelState.Remove(nameof(comment.AppUser));
            ModelState.Remove(nameof(comment.Blog));

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Blog", new { id = comment.BlogId });
            }

            await _commentService.CreateCommentAsync(comment);
            return RedirectToAction("Details", "Blog", new { id = comment.BlogId });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(id);
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (comment == null || comment.AppUserId != currentUserId)
                return Forbid();

            return View(comment);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Comment updatedComment)
        {
            var comment = await _commentService.GetCommentByIdAsync(updatedComment.Id);
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (comment == null || comment.AppUserId != currentUserId)
            {
                return Forbid();
            }

            comment.Text = updatedComment.Text;
            await _unitOfWork.SaveAsync();
            return RedirectToAction("Details", "Blog", new { id = comment.BlogId });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null) return NotFound();

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (comment.AppUserId != currentUserId) return Forbid();

            await _commentService.DeleteCommentAsync(id);
            return RedirectToAction("Details", "Blog", new { id = comment.BlogId });
        }
    }
}
