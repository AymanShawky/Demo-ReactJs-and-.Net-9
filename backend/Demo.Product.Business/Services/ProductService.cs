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

    // get all products with pagination
    public async Task<GetProductsResponse> GetProducts(int pageNumber = 1, int pageSize = 5)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize == default || pageSize > 5) pageSize = 5;

        var totalProductCount = await GetProductCount();
        int totalPageCount = (int)Math.Ceiling((double)totalProductCount / pageSize);

        //validate pageNumber
        if (totalPageCount < pageNumber)
            pageNumber = totalPageCount;

        var products = totalProductCount == 0 ? [] : await dbContext.Products
                                      .Skip((pageNumber - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();
        if (products == null)
            products = [];

        return new GetProductsResponse()
        {
            Products = products.MapToDto(),
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalProductsCount = totalProductCount,
            TotalPagesCount = (int)Math.Ceiling((double)totalProductCount / pageSize)
        };
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


    // update product
    public async Task UpdateProduct(ProductDto model)
    {
        if (model is null)
            throw new ValidationException($"Product cannot by null.");

        var entity = await dbContext.Products.FindAsync(model.Id);

        if (entity is null)
            throw new ValidationException($"Product with id {model.Id} not found");

        entity.Title = model.Title;
        entity.Price = model.Price;
        entity.Description = model.Description;
        entity.Category = model.Category;
        entity.Image = model.ImageUrl;

        dbContext.Products.Update(entity);

        await dbContext.SaveChangesAsync();
    }


    //  get total count of products
    private async Task<int> GetProductCount()
    {
        return await dbContext.Products.CountAsync();
    }
}
