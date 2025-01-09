namespace Demo.Product.Business
{
    public interface IAuthService
    {
        string? Authenticate(string username, string password);
        string? RefreshToken(string token);
    }
}
