using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


namespace Demo.Product.Infrastructure.Entities;

public class Product
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public float Price { get; set; }
    
    public string? Description { get; set; }
    
    public string? Category { get; set; }
    
    public string? Image { get; set; }
    
    public virtual ProductRating? Rating { get; set; }
}


