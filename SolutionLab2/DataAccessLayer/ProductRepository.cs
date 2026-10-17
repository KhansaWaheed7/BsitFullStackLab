using ClassLibraryModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<ProductModel> _products = new()
        {
            new ProductModel { Id = Guid.NewGuid(), Name = "Keyboard", Price = 5000, Category = "Accessories"},
            new ProductModel { Id = Guid.NewGuid(), Name = "Laptop", Price = 120000, Category = "Electronics"},
            new ProductModel { Id = Guid.NewGuid(), Name = "Mouse", Price = 2500, Category = "Accessories"},
           
        };
        public void Add(ProductModel product)
        {
            _products.Add(product);
        }

        public void Delete(Guid id)
        {
            _products.RemoveAll(p => p.Id == id);
        }

        public List<ProductModel> GetAll()
        {
            return _products;
        }

        public ProductModel? GetById(Guid id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void Update(ProductModel product)
        {
            var existingProduct = _products.FirstOrDefault(x => x.Id == product.Id);
            if (existingProduct == null)
                return;
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Category = product.Category;
        }
    }
}
