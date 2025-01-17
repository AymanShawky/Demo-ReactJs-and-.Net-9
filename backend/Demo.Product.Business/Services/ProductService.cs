using Demo.Product.Business.DTOs;
using Demo.Product.Business.Extensions;
using Demo.Product.Business.Interfaces;
using Demo.Product.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;


namespace Demo.Product.Business.Services;
public sealed class ProductService : IProductService
{
    private readonly ILogger<ProductService> logger;
    private readonly AppDbContext dbContext;

    public ProductService(ILogger<ProductService> logger, AppDbContext dbContext)
    {
        this.logger = logger;
        this.dbContext = dbContext;
    }

    // gat all products
    public async Task<IEnumerable<ProductDto>> GetAllProducts()
    {
        var products = await dbContext.Products.ToListAsync();
        
        if (products == null) 
            return Enumerable.Empty<ProductDto>();


        return products.MapToDto();
    }

    // get product by id
    public async Task<ProductDto> GetProductById(int id)
    {
        var product = await dbContext.Products.FindAsync(id);

        if (product is null)
        {
            throw new ValidationException($"Product with id {id} not found");
        }

        return product.MapToDto();
    }

    // add new product
    public async Task AddProduct(ProductDto product)
    {
        if (product is null)
            throw new ValidationException($"Product cannot by null.");

        var entity = product.MapToEntity();
        entity.Rating = new Infrastructure.Entities.ProductRating()
        {
            ProductId = entity.Id,
            Count = 0,
            Rate = 0
        };

        await dbContext.Products.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }
}
