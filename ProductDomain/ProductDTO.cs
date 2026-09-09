using System.ComponentModel.DataAnnotations;
namespace dotnet_example.ProductDomain;

public record ProductDTO(
    [Required][MinLength(1)] string Name,
    [Required][MinLength(1)] string Description,
    [Required][Range(0, double.MaxValue)] decimal Price,
    [Required][Range(0, int.MaxValue)] int Stock
);