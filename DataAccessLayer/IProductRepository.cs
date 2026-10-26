using Model;

namespace DataAccessLayer
{
    public interface IProductRepository
    {
        Task<List<ProductModel>> GetAllAsync();

        Task<ProductModel?> GetByIdAsync(long id);

        Task CreateAsync(ProductModel product);

        Task UpdateAsync(ProductModel product);

        Task DeleteAsync(long id);

        Task<List<ProductModel>> SearchAsync(string keyword);
    }
}