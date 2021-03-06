using System.Collections.Generic;
using System.Linq;
using BedLinenStore.WEB.Enums;
using BedLinenStore.WEB.Models.Entities;

namespace BedLinenStore.WEB.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.MainInfos.Any()) return;

            var mainInfos = new[]
            {
                new MainInfo
                {
                    Name = @"Комплект постельного белья ""Latte""",
                    ImageHoverPath = @"/image/hover/1.jpg",
                    ImagePath = @"/image/1.jpg"
                },
                new MainInfo
                {
                    Name = @"Комплект постельного белья ""Grey""",
                    ImageHoverPath = @"/image/hover/2.jpg",
                    ImagePath = @"/image/2.jpg"
                },
                new MainInfo
                {
                    Name = @"Комплект постельного белья ""Plum""",
                    ImageHoverPath = @"/image/hover/3.jpg",
                    ImagePath = @"/image/3.jpg"
                },
                new MainInfo
                {
                    Name = @"Комплект постельного белья ""Menthol""",
                    ImageHoverPath = @"/image/hover/4.jpg",
                    ImagePath = @"/image/4.jpg"
                },
                new MainInfo
                {
                    Name = @"Комплект постельного белья ""White""",
                    ImageHoverPath = @"/image/hover/5.jpg",
                    ImagePath = @"/image/5.jpg"
                }
            };
            context.MainInfos.AddRange(mainInfos);

            var categories = new[]
            {
                new Category
                {
                    Description = "1,5-спальный: пододеялник: 150*220, простынь: 160*220, наволочки (2 шт): 50*70",
                    Price = 115
                },
                new Category
                {
                    Description = "2,0-спальный: пододеялник: 180*220, простынь: 200*220, наволочки (2 шт): 50*70",
                    Price = 120
                },
                new Category
                {
                    Description = "Евро: пододеялник: 200*220, простынь: 220*240, наволочки (2 шт): 50*70",
                    Price = 130
                },
                new Category
                {
                    Description = "Семейный: пододеялник (2 шт): 150*220, простынь: 220*240, наволочки (2 шт): 50*70",
                    Price = 140
                }
            };

            context.Categories.AddRange(categories);

            var products = new List<Product>();
            foreach (var mainInfo in mainInfos)
            foreach (var category in categories)
                products.Add(new Product
                {
                    Category = category,
                    MainInfo = mainInfo
                });

            context.Products.AddRange(products);

            var user = new User
            {
                Email = "admin123@admin.com",
                Password = "Admin12345",
                Role = Role.Admin,
                ConfirmedEmail = true
            };

            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}