namespace Demo.Product.Infrastructure.Entities;

public class ProductRating
{
    public int Id { get; set; }
    public float Rate { get; set; }
    public int Count { get; set; }
    public int ProductId { get; set; }

    public virtual Product? Product { get; set; }
}


