using Ciber.Data;
using Microsoft.EntityFrameworkCore;

namespace Ciber.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();

                // Look for any cate.
                if (context.Order.Any())
                {
                    return;   // DB has been seeded
                }

                context.Users.Add(
                    new Microsoft.AspNetCore.Identity.IdentityUser
                    {
                        UserName = "test1@example.com",
                        NormalizedUserName = "TEST1@EXAMPLE.COM",
                        Email = "test1@example.com",
                        NormalizedEmail = "TEST1@EXAMPLE.COM",
                        EmailConfirmed = true,
                        PasswordHash = "AQAAAAEAACcQAAAAEKwBygPSfqYvVj0VLDFV2I+yvQe/voVE+lrpPfNNhxTsrp4f7+5I0ZcNitHC4Z2oaw==",
                        SecurityStamp = "EPZLRD42QIPZIHJPVYMI7HP4TRY7UUJV",
                        ConcurrencyStamp = "b71839f9-9ce7-4234-b9d4-7202f4a2629b",
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        LockoutEnabled = true,
                        AccessFailedCount = 0,
                    }
                    );
                context.SaveChanges();

                context.Category.AddRange(
                    new Category
                    {
                        Name = "Cat1",
                        Description = "Category 1"
                    },
                    new Category
                    {
                        Name = "Cat2",
                        Description = "Category 2"
                    },
                    new Category
                    {
                        Name = "Cat3",
                        Description = "Category 3"
                    },
                    new Category
                    {
                        Name = "Cat4",
                        Description = "Category 4"
                    }
                );
                context.SaveChanges();

                context.Customer.AddRange(
                    new Customer
                    {
                        Name = "Mr Hung",
                        Address = "Ha Noi"
                    },
                    new Customer
                    {
                        Name = "Mr Binh",
                        Address = "Ho Chi Minh"
                    },
                    new Customer
                    {
                        Name = "Mr Son",
                        Address = "Vinh"
                    }
                );
                context.SaveChanges();

                context.Product.AddRange(
                    new Product
                    {
                        Name = "Pro1",
                        CategoryId = context.Category.FirstOrDefault().Id,
                        Description = "Production 1",
                        Price = 10000,
                        Quantity = 100
                    },
                    new Product
                    {
                        Name = "Pro2",
                        CategoryId = context.Category.Where(x => x.Id == 2).Select(x => x.Id).FirstOrDefault(),
                        Description = "Production 2",
                        Price = 10000,
                        Quantity = 100
                    },
                    new Product
                    {
                        Name = "Pro3",
                        CategoryId = context.Category.Where(x => x.Id == 3).Select(x => x.Id).FirstOrDefault(),
                        Description = "Production 3",
                        Price = 10000,
                        Quantity = 100
                    }
                );
                context.SaveChanges();

                context.Order.AddRange(
                    new Order
                    {
                        Name="Ord1",
                        CustomerId = 1,
                        ProductId = 1,
                        Amount = 2,
                        OrderDate = DateTime.Now
                    },
                    new Order
                    {
                        Name = "Ord2",
                        CustomerId = 2,
                        ProductId = 2,
                        Amount = 2,
                        OrderDate = DateTime.Now
                    },
                    new Order
                    {
                        Name = "Ord3",
                        CustomerId = 3,
                        ProductId = 3,
                        Amount = 2,
                        OrderDate = DateTime.Now
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
