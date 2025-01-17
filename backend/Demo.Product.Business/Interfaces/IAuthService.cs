using Demo.Product.Business.DTOs;

namespace Demo.Product.Business;

public interface IAuthService
{
    Task<AuthResponseDto> Authenticate(string username, string password);
    AuthResponseDto? RefreshToken(string token);
}
