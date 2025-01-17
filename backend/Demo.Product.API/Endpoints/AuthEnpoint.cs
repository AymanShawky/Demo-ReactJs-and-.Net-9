using Demo.Product.Business;
using Demo.Product.Business.DTOs;
using Demo.Product.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Product.API.Endpoints
{
    public static class AuthEnpoint
    {

        public record AuthenticateRequest(string Username, string Password);

        public static void RegisterAuthEnpoints(this WebApplication app)
        {
            app.MapPost("/auth/login", async (AuthenticateRequest request, [FromServices] IAuthService authService) =>
            {
                if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return Results.BadRequest("Invalid request data");
                }

                var authResponse = await authService.Authenticate(request.Username, request.Password);
                if (authResponse == null)
                {
                    return Results.Unauthorized();
                }

                return Results.Ok(authResponse);
            });

            app.MapPost("/auth/refresh", (string refreshToken, [FromServices]IAuthService authService) =>
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
