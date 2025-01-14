namespace Demo.Product.Infrastructure.Entities;

public class  Role
{
    public int Id { get; set; }

    public string RoleName { get; set; }

    public string? RoleDescription { get; set; }

    public virtual List<User>? Users { get; set; }
}
