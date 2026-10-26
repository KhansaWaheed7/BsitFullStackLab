using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductModel> Products => Set<ProductModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductModel>(entity =>
            {
                // Map to the existing Supabase table and column names.
   
                entity.ToTable("Products", "public");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                    .HasColumnName("Id")
                    .UseIdentityByDefaultColumn();

                entity.Property(p => p.Name)
                    .HasColumnName("Name")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(p => p.Price)
                    .HasColumnName("Price")
                    .HasColumnType("bigint")
                    .IsRequired();

                entity.Property(p => p.Category)
                    .HasColumnName("Category")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(p => p.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(500)
                    .IsRequired(false);

                entity.Property(p => p.IsActive)
                    .HasColumnName("IsActive")
                    .HasDefaultValue(true)
                    .IsRequired();
            });
        }
    }
}
