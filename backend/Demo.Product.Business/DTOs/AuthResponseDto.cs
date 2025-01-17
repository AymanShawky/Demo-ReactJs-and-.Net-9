
namespace Demo.Product.Business.DTOs;

public class AuthResponseDto
{
    public AuthResponseDto(string token, string refreshToken)
    {
        Token = token; ;
        RefreshToken = refreshToken;
    }

    public string Token { get; set; }

    public string RefreshToken { get; set; }
}
