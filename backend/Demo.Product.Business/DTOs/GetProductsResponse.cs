namespace Demo.Product.Business.DTOs;

public sealed class GetProductsResponse
{
    public required IEnumerable<ProductDto> Products { get; set; }

    public int TotalProductsCount { get; set; }

    public int TotalPagesCount { get; set; }

    public int CurrentPage { get; set; }

    public int PageSize { get; set; }
}
