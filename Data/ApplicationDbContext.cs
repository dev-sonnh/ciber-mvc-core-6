using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ciber.Models;

namespace Ciber.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Ciber.Models.Order>? Order { get; set; }
        public DbSet<Ciber.Models.Category>? Category { get; set; }
        public DbSet<Ciber.Models.Customer>? Customer { get; set; }
        public DbSet<Ciber.Models.Product>? Product { get; set; }
        

    }
}