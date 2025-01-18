using Demo.Product.Business.DTOs;
using Demo.Product.Infrastructure;
using Demo.Product.Infrastructure.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo.Product.Business.Services;

public sealed class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _dbContext;
    private static readonly Dictionary<string, string> _dicRefreshTokens = new();

    public AuthService(IConfiguration configuration, AppDbContext dbContext)
    {
        this._configuration = configuration;
        _dbContext = dbContext;
    }

    public async Task<AuthResponseDto> Authenticate(string username, string password)
    {
        // Validate the user credentials
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            throw new ValidationException("Invalid username or password");
        }

        // Validate the password
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            throw new ValidationException("Invalid username or password");
        }

        var tokenString = GenerateJwtToken(user.Id);

        // Generate and store refresh token
        var refreshToken = Guid.NewGuid().ToString();
        _dicRefreshTokens[refreshToken] = username;

        
        return new AuthResponseDto(tokenString, refreshToken);
    }

    public AuthResponseDto? RefreshToken(string refreshToken)
    {
        if (_dicRefreshTokens.TryGetValue(refreshToken, out var username))
        {
            int userId = _dbContext.Users
                .Where(u => u.Username == username)
                .Select(u => u.Id)
                .FirstOrDefault();

            string newTokenString = GenerateJwtToken(userId);

            // Optionally, generate a new refresh token
            var newRefreshToken = Guid.NewGuid().ToString();
            _dicRefreshTokens.Remove(refreshToken);
            _dicRefreshTokens[newRefreshToken] = username;

            return new AuthResponseDto(newTokenString, refreshToken);
        }

        return null;
    }

    private string GenerateJwtToken(int userId)
    {
        // asume the user has only one role
        var role = _dbContext.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.RoleName)
            .FirstOrDefault();

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokeOptions = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: new List<Claim>() 
            {
                new Claim(ClaimTypes.Role, value: role) 
            },
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: signinCredentials
        );

        var newTokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        return newTokenString;
    }
}
