using BlogAppp.IServices;
using BlogAppp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAppp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;

        public BlogController(IBlogService blogService, ICategoryService categoryService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int? categoryId, string? search)
        {
            List<Blog> blogs;

            if (!string.IsNullOrWhiteSpace(search))
            {
                blogs = await _blogService.SearchBlogsAsync(search);
            }
            else
            {
                blogs = await _blogService.GetAllBlogsAsync(categoryId);
            }

            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.SearchTerm = search;
            return View(blogs);
        }

        public async Task<IActionResult> Details(int id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            if (blog == null) return NotFound();
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View(blog);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Blog blog, IFormFile image)
        {
            ModelState.Remove("image");
            var userId  = User.Identity?.IsAuthenticated == true
                ? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                : null;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            blog.AppUserId = userId;
            ModelState.Remove(nameof(blog.AppUserId));
            ModelState.Remove(nameof(blog.AppUser));
            ModelState.Remove(nameof(blog.Category));

            if (ModelState.IsValid)
            {
                blog.PublishDate = DateTime.Now;

                if (image != null)
                {
                    var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(imageFolder))
                        Directory.CreateDirectory(imageFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var imagePath = Path.Combine(imageFolder, uniqueFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    blog.ImagePath = "/images/" + uniqueFileName;
                }
                else
                {
                    blog.ImagePath = "/images/default.jpg"; 
                }

                await _blogService.CreateBlogAsync(blog);
                return RedirectToAction("Index");
            }

            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View(blog);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            if (blog == null) return NotFound();

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (blog.AppUserId != currentUserId) return Forbid();

            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View(blog);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Blog blog, IFormFile? image)
        {
            var existingBlog = await _blogService.GetBlogByIdAsync(blog.Id);
            if (existingBlog == null) return NotFound();

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (existingBlog.AppUserId != currentUserId) return Forbid();

            ModelState.Remove(nameof(blog.AppUser));
            ModelState.Remove(nameof(blog.AppUserId));
            ModelState.Remove(nameof(blog.Category));
            ModelState.Remove(nameof(blog.Comments));

            if (ModelState.IsValid)
            {
                existingBlog.Title = blog.Title;
                existingBlog.Content = blog.Content;
                existingBlog.CategoryId = blog.CategoryId;

                if (image != null)
                {
                    var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(imageFolder))
                        Directory.CreateDirectory(imageFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var imagePath = Path.Combine(imageFolder, uniqueFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    existingBlog.ImagePath = "/images/" + uniqueFileName;
                }

                await _blogService.UpdateBlogAsync(existingBlog);
                return RedirectToAction("Details", new { id = blog.Id });
            }

            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return RedirectToAction("Details", new { id = blog.Id });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            if (blog == null) return NotFound();

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (blog.AppUserId != currentUserId) return Forbid();

            await _blogService.DeleteBlogAsync(id);
            return RedirectToAction("Index");
        }
    }
}
