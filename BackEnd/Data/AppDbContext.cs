using ProductManagement.Models;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Models;

namespace ProductManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<CategoryMst> CategoryMst { get; set; }
        public DbSet<BrandMst> BrandMst { get; set; }
        public DbSet<ProductMst> ProductMst { get; set; }

        public DbSet<CategoryBrandMapping> CategoryBrandMapping { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductMst>(entity =>
            {
                entity.HasKey(e => e.Product_Id);

                entity.Property(e => e.Mfg_Date)
                    .HasColumnType("date");

                entity.Property(e => e.Expiry_Date)
                    .HasColumnType("date");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)");

                // Configure foreign keys WITHOUT navigation properties
                entity.HasOne<CategoryMst>()
                    .WithMany()
                    .HasForeignKey(e => e.Category_Id)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<BrandMst>()
                    .WithMany()
                    .HasForeignKey(e => e.Brand_Id)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
