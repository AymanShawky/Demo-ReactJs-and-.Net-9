using Demo.Product.Business;

namespace Demo.Product.API.Endpoints
{
    public static class ProductEnpoint
    {

        public record ProductRequest(string productId);
        public record ProductResponse(string productName);

        public static void RegisterProductEnpoint(this WebApplication app)
        {
            app.MapGet("/Product",  () =>
            {
                return Results.Ok(new ProductResponse("token"));
            });

            app.MapPost("/Product",  (ProductRequest request) =>
            {
                return Results.Ok(new ProductResponse("token"));
            })
            .RequireAuthorization();

        }
    }


}
