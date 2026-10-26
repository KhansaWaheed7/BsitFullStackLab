using ClassLibraryModel;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public void Create(ProductModel product)
        {
            _productRepository.Add(product);
        }

        public void Delete(Guid id)
        {
            _productRepository.Delete(id);
        }

        public List<ProductModel> GetAll()
        {
           return _productRepository.GetAll();
        }

        public ProductModel? GetById(Guid id)
        {
            return _productRepository.GetById(id);
        }

        public void Update(ProductModel product)
        {
            _productRepository.Update(product);
        }
    }
}
