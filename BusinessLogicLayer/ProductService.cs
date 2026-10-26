using DataAccessLayer;
using Model;

namespace BusinessLogicLayer
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<List<ProductModel>> GetAllAsync()
        {
            return await productRepository.GetAllAsync();
        }

        public async Task<ProductModel?> GetByIdAsync(long id)
        {
            return await productRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(ProductModel product)
        {
            Validate(product);
            await productRepository.CreateAsync(product);
        }

        public async Task UpdateAsync(ProductModel product)
        {
            if (product.Id <= 0)
            {
                throw new ArgumentException("Invalid product.");
            }

            Validate(product);
            await productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(long id)
        {
            await productRepository.DeleteAsync(id);
        }

        public async Task<List<ProductModel>> SearchAsync(string keyword)
        {
            return await productRepository.SearchAsync(keyword ?? "");
        }

        // Validation problems throw ArgumentException so the UI can show
        // them as friendly messages (database errors are different exceptions).
        private static void Validate(ProductModel product)
        {
            product.Name = (product.Name ?? "").Trim();
            product.Category = (product.Category ?? "").Trim();
            product.Description = (product.Description ?? "").Trim();

            if (product.Name.Length == 0)
            {
                throw new ArgumentException("Product name is required.");
            }

            if (product.Name.Length > 200)
            {
                throw new ArgumentException("Product name cannot exceed 200 characters.");
            }

            if (product.Price < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }

            if (product.Category.Length == 0)
            {
                throw new ArgumentException("Category is required.");
            }

            if (product.Category.Length > 100)
            {
                throw new ArgumentException("Category cannot exceed 100 characters.");
            }

            if (product.Description.Length > 500)
            {
                throw new ArgumentException("Description cannot exceed 500 characters.");
            }
        }
    }
}