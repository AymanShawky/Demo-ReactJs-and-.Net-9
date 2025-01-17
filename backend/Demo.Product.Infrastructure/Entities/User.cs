namespace Demo.Product.Infrastructure.Entities;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string PasswordHash { get; set; }

    public virtual List<UserRole>? UserRoles { get; set; }
    public virtual List<Role>? Roles { get; set; }
}
