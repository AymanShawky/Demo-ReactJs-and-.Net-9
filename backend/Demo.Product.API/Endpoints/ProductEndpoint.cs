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
        var productGroup = app.MapGroup("/product");
        var productGroupWithId = productGroup.MapGroup("/{productId:int}");

        // getting product by id
        productGroupWithId.MapGet("", async Task<Results<NotFound, Ok<ProductDto>>> (int productId, IProductService productService) =>
        {
            var product = await productService.GetProductById(productId);

            if (product == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(product);
        })
            .WithName("GetProductById")
            .WithOpenApi()
            .WithSummary("Get product by id")
            .WithDescription("Get product by providing an id")
            .RequireAuthorization("UserOrAdmin");

        // get all products with pagination
        productGroup.MapGet("", async Task<Ok<GetProductsResponse>> (int page, int size, IProductService productService) =>
        {
            var productsResponse = await productService.GetProducts(page, size);

            return TypedResults.Ok(productsResponse);
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

        // update product
        productGroupWithId.MapPut("", async (int productId, [FromBody] ProductDto model, IProductService productService) =>
        {
            await productService.UpdateProduct(model);
            var updatedProduct = await productService.GetProductById(model.Id);
            return Results.Ok(updatedProduct);
        })
            .WithName("UpdateProduct")
            .RequireAuthorization("Admin");
    }
}
