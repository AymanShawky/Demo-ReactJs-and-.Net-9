namespace Demo.Product.Business.DTOs;

public class ProductDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public float Price { get; set; }

    public string? Description { get; set; }

    public string? Category { get; set; }

    public string? ImageUrl { get; set; }
}
