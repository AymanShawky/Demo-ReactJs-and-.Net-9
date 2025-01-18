using Demo.Product.Business.DTOs;
using Demo.Product.Business.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Product.API.Endpoints;

public static class ProductEnpoint
{

    public record ProductRequest(int productId);
    public record ProductResponse(string ProductName);

    public static void RegisterProductEnpoint(this WebApplication app)
    {
        var productGroup = app.MapGroup("/products");

        // getting product by id
        productGroup.MapGet("/{productId}", async Task<Results<NotFound, Ok<ProductDto>>> (int productId, IProductService productService) =>
        {
            var product = await productService.GetProductById(productId);

            if (product == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(product);
        })
            .WithName("GetProductById")
            .RequireAuthorization("UserOrAdmin");

        // get all products with pagination
        productGroup.MapGet("", async (int pageNumber, int pageSize, IProductService productService) =>
        {
            var products = await productService.GetAllProducts();
            var paginatedProducts = products.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return Results.Ok(paginatedProducts);
        })
            .WithName("GetAllProducts")
            .RequireAuthorization("UserOrAdmin");

        //create new product
        productGroup.MapPost("", async ([FromBody] ProductDto model, IProductService productService) =>
        {
            await productService.AddProduct(model);
            var createdProduct = await productService.GetProductById(model.Id);

            return Results.Created($"/products/{createdProduct.Id}", createdProduct);
        })
            .WithName("CreateProduct")
            .RequireAuthorization("Admin");
    }
}
