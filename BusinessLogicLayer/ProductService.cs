using DataAccessLayer;
using Model;

namespace BusinessLogicLayer
{
    public class ProductService : IProductService
    {

        public List<ProductModel> GetAllData()
        {
            return CRUD.GetAllData();
        }
































        private readonly IProductRepository productRepository;
       

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<List<ProductModel>> GetAllAsync()
        {
            return await productRepository.GetAllAsync();
        }

        public async Task<ProductModel?> GetByIdAsync(Guid id)
        {
            return await productRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(ProductModel product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new Exception("Product name is required.");
            }

            if (product.Price < 0)
            {
                throw new Exception("Price cannot be negative.");
            }

            if (string.IsNullOrWhiteSpace(product.Category))
            {
                throw new Exception("Category is required.");
            }

            await productRepository.CreateAsync(product);
        }

        public async Task UpdateAsync(ProductModel product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new Exception("Product name is required.");
            }

            if (product.Price < 0)
            {
                throw new Exception("Price cannot be negative.");
            }

            if (string.IsNullOrWhiteSpace(product.Category))
            {
                throw new Exception("Category is required.");
            }

            await productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            await productRepository.DeleteAsync(id);
        }

        public async Task<List<ProductModel>> SearchAsync(string keyword)
        {
            return await productRepository.SearchAsync(keyword);
        }
    }
}