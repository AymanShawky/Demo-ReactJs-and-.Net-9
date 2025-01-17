using Demo.Product.Business;
using Demo.Product.Business.DTOs;
using Demo.Product.Business.Interfaces;
using Demo.Product.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Product.API.Endpoints;

public static class ProductEnpoint
{

    public record ProductRequest(int productId);
    public record ProductResponse(string ProductName);

    public static void RegisterProductEnpoint(this WebApplication app)
    {
       
        // getting product by id
        app.MapGet("/Product/{productId}", async (int productId, IProductService productService) =>
        {
            var product = await productService.GetProductById(productId);
            return Results.Ok(new ProductResponse(product.Title));
        })
            .WithName("GetProductById")
            .RequireAuthorization("User");

        // get all products
        app.MapGet("/Product", async (IProductService productService) =>
        {
            var products = await productService.GetAllProducts();

            return Results.Ok(products);
        })
            .WithName("GetAllProducts");
           // .RequireAuthorization("User"); ;

        //create new product
        app.MapPost("/product", async ([FromBody] ProductDto model, IProductService productService) =>
        {
            await productService.AddProduct(model);

            return Results.Ok();
        })
            .WithName("CreateProduct")
            .RequireAuthorization("Admin");
        
    }
}
