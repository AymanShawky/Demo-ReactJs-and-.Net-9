using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo.Product.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        private static readonly Dictionary<string, string> _dicRefreshTokens = new();

        public AuthService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string? Authenticate(string username, string password) 
        {
            // Validate the user credentials (this is just a demo, replace with your own logic)
            if (username == "test" && password == "password")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: signinCredentials
                );

                 var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                // Generate and store refresh token
                var refreshToken = Guid.NewGuid().ToString();
                _dicRefreshTokens[refreshToken] = username;

                return tokenString + ";" + refreshToken;
            }

            return null;

        }

        public string? RefreshToken(string token)
        {
            if (_dicRefreshTokens.TryGetValue(token, out var username))
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: signinCredentials
                );

                var newTokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                // Optionally, generate a new refresh token
                var newRefreshToken = Guid.NewGuid().ToString();
                _dicRefreshTokens.Remove(token);
                _dicRefreshTokens[newRefreshToken] = username;

                return newTokenString + ";" + newRefreshToken;
            }

            return null;
        }


    }
}
