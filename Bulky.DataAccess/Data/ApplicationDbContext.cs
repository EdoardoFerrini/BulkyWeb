
using Bulky.Models;
using Microsoft.EntityFrameworkCore;
namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Sci-Fi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Title= "Scarface", Author="Brian De Palma", ISBN="WDSG523", ListPrice = 70, Price = 60, Price50 = 40, Price100 = 20, Description="One of the most famous Mafia movie", CategoryId = 1, ImageUrl="" },
                new Product { Id = 2, Title = "Interstellar", Author = "Christopher Nolan", ISBN = "DVN098", ListPrice = 70, Price = 60, Price50 = 40, Price100 = 20, Description = "One of the most famous Sci-Fi movie", CategoryId = 2, ImageUrl="" }
                );
        }
    }
}
