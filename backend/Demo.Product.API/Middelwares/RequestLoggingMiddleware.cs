using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Demo.Product.API.Middelwares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly IConfiguration _configuration;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log request headers
            _logger.LogInformation("Incoming Request: {method} {url}", context.Request.Method, context.Request.Path);
            foreach (var header in context.Request.Headers)
            {
                _logger.LogInformation("Header: {key}: {value}", header.Key, header.Value);
            }

            // Log request body
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;
            _logger.LogInformation("Request Body: {body}", body);

            // Validate JWT token
            if (context.Request.Headers.TryGetValue("Authorization", out var token))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                try
                {
                    string authToken = token.ToString().Replace("Bearer ", "").Split(';')[0];
                    tokenHandler.ValidateToken(authToken , new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    }, out SecurityToken validatedToken);

                    _logger.LogInformation("Token is valid.");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Token validation failed: {message}", ex.Message);
                }
            }
            else
            {
                _logger.LogWarning("Authorization header not found.");
            }

            await _next(context);
        }
    }
}