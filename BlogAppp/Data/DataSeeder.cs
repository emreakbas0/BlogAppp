using BlogAppp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogAppp.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(AppDbContext context, UserManager<AppUser> userManager)
        {
            
            await context.Database.MigrateAsync();

            
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Teknoloji" },
                    new Category { Name = "Yazılım" },
                    new Category { Name = "Gündem" },
                    new Category { Name = "Tasarım" },
                    new Category { Name = "Spor" }
                );
                await context.SaveChangesAsync();
            }

            
            var defaultUser = await userManager.FindByNameAsync("demo");
            if (defaultUser == null)
            {
                defaultUser = new AppUser
                {
                    UserName = "demo",
                    Email = "demo@example.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(defaultUser, "Demo123.");
            }

            
            if (!context.Blogs.Any())
            {
                var teknoloji = context.Categories.FirstOrDefault(c => c.Name == "Teknoloji");
                var yazılım = context.Categories.FirstOrDefault(c => c.Name == "Yazılım");
                var gündem = context.Categories.FirstOrDefault(c => c.Name == "Gündem");
                var tasarım = context.Categories.FirstOrDefault(c => c.Name == "Tasarım");
                var spor = context.Categories.FirstOrDefault(c => c.Name == "Spor");
                var category = context.Categories.OrderBy(c => c.Id).First();
                context.Blogs.AddRange(
                    new Blog
                    {
                        Title = "Spor Yazısı",
                        Content = "Bu bir örnek spor yazısıdır.",
                        CategoryId = spor?.Id ?? category.Id,
                        AppUserId = defaultUser.Id,
                        PublishDate = DateTime.Now,
                        ImagePath = "/images/spor.jpg"
                    },
                    new Blog
                    {
                        Title = "Teknoloji Yazısı",
                        Content = "Bu da teknoloji yazısıdır.",
                        CategoryId = teknoloji?.Id ?? category.Id,
                        AppUserId = defaultUser.Id,
                        PublishDate = DateTime.Now.AddDays(-1),
                        ImagePath = "/images/teknoloji.jpg"
                    },
                    new Blog
                    {
                        Title = "Tasarım Yazısı",
                        Content = "Bu da test tasarım blog yazısıdır.",
                        CategoryId = tasarım?.Id ?? category.Id,
                        AppUserId = defaultUser.Id,
                        PublishDate = DateTime.Now.AddDays(-1),
                        ImagePath = "/images/tasarım.jpg"
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
