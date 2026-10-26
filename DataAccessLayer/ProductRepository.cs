using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccessLayer
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public ProductRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<ProductModel>> GetAllAsync()
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            return await db.Products
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .ToListAsync();
        }

        public async Task<ProductModel?> GetByIdAsync(long id)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            return await db.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateAsync(ProductModel product)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            db.Products.Add(product);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductModel product)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var existingProduct = await db.Products
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct is null)
            {
                throw new InvalidOperationException(
                    "Product not found. It may have been deleted.");
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Category = product.Category;
            existingProduct.Description = product.Description;
            existingProduct.IsActive = product.IsActive;

            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            var product = await db.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                return;
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();
        }

        public async Task<List<ProductModel>> SearchAsync(string keyword)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await db.Products
                    .AsNoTracking()
                    .OrderBy(p => p.Id)
                    .ToListAsync();
            }

            keyword = keyword.Trim();

            return await db.Products
                .AsNoTracking()
                .Where(p =>
                    EF.Functions.ILike(p.Name, $"%{keyword}%") ||
                    EF.Functions.ILike(p.Category, $"%{keyword}%") ||
                    (p.Description != null &&
                     EF.Functions.ILike(p.Description, $"%{keyword}%")))
                .OrderBy(p => p.Id)
                .ToListAsync();
        }
    }
}
