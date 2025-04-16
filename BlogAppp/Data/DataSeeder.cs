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
                    new Category { Name = "Yapay Zeka" },
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
                var yapayzeka = context.Categories.FirstOrDefault(c => c.Name == "Yapay Zeka");
                var spor = context.Categories.FirstOrDefault(c => c.Name == "Spor");
                var category = context.Categories.OrderBy(c => c.Id).First();
                context.Blogs.AddRange(
                    new Blog
                    {
                        Title = "Havvanur Kethüda: Türkiye’nin Dünya Şampiyonu Sporcusu",
                        Content = "Milli boksör Havvanur Kethüda, Dünya Boks Birliği Şampiyonası’nda altın madalya kazanarak bir kez daha dünya şampiyonu oldu. Karadağ’ın Budva kentinde düzenlenen şampiyonada finalde Kazak rakibi Sabina Zhaksylyk’i mağlup eden Kethüda, büyük bir zafer kazandı.",
                        CategoryId = spor?.Id ?? category.Id,
                        AppUserId = defaultUser.Id,
                        PublishDate = DateTime.Now,
                        ImagePath = "/images/sport.jpg"
                    },
                    new Blog
                    {
                        Title = "DeepSeek R1: Verimliliği Yeniden Tanımlayan Yapay Zekâ Modeli",
                        Content = "Son dönemde yapay zekâ dünyasında adından sıkça söz ettiren DeepSeek, açık kaynaklı stratejisi, güçlü performansı ve yenilikçi modelleriyle gündemdeki yerini aldı. Özellikle DeepSeek-R1 modelinin piyasaya sürülmesiyle, sektör devlerine meydan okuyan şirket, kısa sürede global teknoloji dünyasında büyük bir etki yarattı. Çin’in Hangzhou kentinde, Temmuz 2023’te kurulan DeepSeek, yapay zekâ alanındaki yenilikçi yaklaşımlarıyla sektöre hızlı bir giriş yaptı. Şirketin kurucusu Liang Wenfeng, aynı zamanda High-Flyer adlı bir hedge fonunun da kurucusudur ve bu fon, DeepSeek'in ilk destekçilerinden biri olarak şirketin doğuşunda kritik bir rol oynamıştır. Liang’ın hedefi, yapay genel zekâ (AGI) seviyesine ulaşarak insan yeteneklerini eşitleyebilen ya da aşan modeller geliştirmektir.",
                        CategoryId = yapayzeka?.Id ?? category.Id,
                        AppUserId = defaultUser.Id,
                        PublishDate = DateTime.Now.AddDays(-1),
                        ImagePath = "/images/yapayzeka.jpg"
                    },
                    new Blog
                    {
                        Title = "Hizmet Sektöründe Yapay Zeka Entegrasyonu: Müşteri Deneyimini Dönüştüren Teknolojik Yenilikler",
                        Content = "Yapay zeka , son yıllarda hizmet sektöründe büyük bir dönüşüm yaratıyor. İşletmeler, müşteri deneyimini iyileştirmek, iş süreçlerini daha verimli hale getirmek ve rekabet avantajı sağlamak amacıyla yapay zeka teknolojilerine hızla yatırım yapıyor. Ancak, bu teknolojinin entegrasyonu her zaman kolay olmuyor. Yapay zekanın potansiyelinden faydalanabilmek için işletmelerin doğru stratejilerle ilerlemesi, başarılı örnekleri analiz etmesi ve müşterilerinin ihtiyaçlarına uygun çözümler geliştirmesi kritik önem taşıyor.",
                        CategoryId = yazılım?.Id ?? category.Id,
                        AppUserId = defaultUser.Id,
                        PublishDate = DateTime.Now.AddDays(-1),
                        ImagePath = "/images/yazılım.jpg"
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
