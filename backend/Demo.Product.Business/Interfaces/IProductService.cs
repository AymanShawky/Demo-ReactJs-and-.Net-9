using Demo.Product.Business.DTOs;

namespace Demo.Product.Business.Interfaces;

public interface IProductService
{
    Task AddProduct(ProductDto product);
    Task<GetProductsResponse> GetProducts(int pageNumber, int pageSize);
    Task<ProductDto> GetProductById(int id);
    Task UpdateProduct(ProductDto model);
}