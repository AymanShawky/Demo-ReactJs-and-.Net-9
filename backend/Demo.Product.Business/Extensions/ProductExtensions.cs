using Demo.Product.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Product.Business.Extensions;
internal static class ProductExtensions
{
    // extension method to convert Entities.Product to PRoductDto

    internal static ProductDto MapToDto(this Infrastructure.Entities.Product product)
    {
        if (product is null)
            return new ProductDto();

        return new ProductDto()
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category,
            ImageUrl = product.Image
        };
    }


    // Extension method to convert List<Entities.Product> to List<ProductDto>
    internal static List<ProductDto> MapToDto(this List<Infrastructure.Entities.Product> products)
    {
        return 
        products == null ? 
        [] : products.Select(product => product.MapToDto()).ToList();
    }

    // Extension method to convert ProductDto to Entities.Product
    internal static Infrastructure.Entities.Product MapToEntity(this ProductDto productDto)
    {
        if (productDto is null)
            return new Infrastructure.Entities.Product();

        return new Infrastructure.Entities.Product()
        {
            Id = productDto.Id,
            Title = productDto.Title,
            Description = productDto.Description,
            Price = productDto.Price,
            Category = productDto.Category,
            Image = productDto.ImageUrl
        };
    }
}
