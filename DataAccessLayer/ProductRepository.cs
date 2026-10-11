using System.Text.Json;
using Model;

namespace DataAccessLayer
{
    public class ProductRepository : IProductRepository
    {
        private readonly string filePath;


        public ProductRepository()
        {
            filePath = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "products.json"
            );
        }


        public async Task<List<ProductModel>> GetAllAsync()
        {
            if (!File.Exists(filePath))
            {
                return new List<ProductModel>();
            }

            string json = await File.ReadAllTextAsync(filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<ProductModel>();
            }


            return JsonSerializer.Deserialize<List<ProductModel>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })
                ?? new List<ProductModel>();

        }

        public async Task<ProductModel?> GetByIdAsync(Guid id)
        {
            List<ProductModel> products = await GetAllAsync();

            return products.FirstOrDefault(p => p.Id == id);
        }

        public async Task CreateAsync(ProductModel product)
        {
            List<ProductModel> products = await GetAllAsync();

            if (product.Id == Guid.Empty)
            {
                product.Id = Guid.NewGuid();
            }

            products.Add(product);

            await SaveProductsAsync(products);
        }

        public async Task UpdateAsync(ProductModel product)
        {
            List<ProductModel> products = await GetAllAsync();

            ProductModel? existingProduct =
                products.FirstOrDefault(p => p.Id == product.Id);

            if (existingProduct == null)
            {
                throw new Exception("Product not found.");
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Category = product.Category;
            existingProduct.Description = product.Description;
            existingProduct.IsActive = product.IsActive;

            await SaveProductsAsync(products);
        }

        public async Task DeleteAsync(Guid id)
        {
            List<ProductModel> products = await GetAllAsync();

            ProductModel? product =
                products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                products.Remove(product);

                await SaveProductsAsync(products);
            }
        }

        public async Task<List<ProductModel>> SearchAsync(string keyword)
        {
            List<ProductModel> products = await GetAllAsync();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                return products;
            }

            keyword = keyword.ToLower();

            return products.Where(p =>
                p.Name.ToLower().Contains(keyword) ||
                p.Category.ToLower().Contains(keyword) ||
                p.Description.ToLower().Contains(keyword)
            ).ToList();
        }

        private async Task SaveProductsAsync(List<ProductModel> products)
        {
            string json = JsonSerializer.Serialize(
                products,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            await File.WriteAllTextAsync(filePath, json);
        }
    }
}