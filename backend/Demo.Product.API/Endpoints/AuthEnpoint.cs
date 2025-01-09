using Demo.Product.Business;

namespace Demo.Product.API.Endpoints
{
    public static class AuthEnpoint
    {

        public record AuthenticateRequest(string Username, string Password);
        public record AuthenticateResponse(string Token);

        public static void RegisterAuthEnpoint(this WebApplication app, IAuthService authService)
        {
            app.MapPost("/auth", async (AuthenticateRequest request) =>
            {
                if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return Results.BadRequest("Invalid request data");
                }

                var token = authService.Authenticate(request.Username, request.Password);
                if (token == null)
                {
                    return Results.Unauthorized();
                }
                return Results.Ok(new AuthenticateResponse(token));
            });

            app.MapPost("/auth/refresh", (string refreshToken) =>
            {
                var newToken = authService.RefreshToken(refreshToken);
                if (newToken == null)
                {
                    return Results.Unauthorized();
                }
                return Results.Ok(newToken);
            });


        }
    }


}
