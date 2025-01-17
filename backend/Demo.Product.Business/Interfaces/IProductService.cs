using Demo.Product.Business.DTOs;

namespace Demo.Product.Business.Interfaces;

public interface IProductService
{
    Task AddProduct(ProductDto product);
    Task<IEnumerable<ProductDto>> GetAllProducts();
    Task<ProductDto> GetProductById(int id);
}