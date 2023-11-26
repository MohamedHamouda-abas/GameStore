using BULKYMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BULKYMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(

                new Category { Id = 1, Name = "Actions", DisPlayOrder = 1 },
                new Category { Id = 2, Name = "Actions", DisPlayOrder = 2 },
                new Category { Id = 3, Name = "Actions", DisPlayOrder = 3 }
              );
        }
    }
}
