using dotnet_example.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_example.ProductDomain;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        return await _context.Products.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Add(ProductDTO productDTO)
    {
        var product = new Product
        {
            Name = productDTO.Name,
            Description = productDTO.Description,
            Price = productDTO.Price,
            Stock = productDTO.Stock
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
        nameof(GetById),
        new { id = product.Id },
        product
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductDTO productDTO)
    {
        var existingProduct = await _context.Products.FindAsync(id);

        if (existingProduct is null)
        {
            return NotFound();
        }

        existingProduct.Name = productDTO.Name;
        existingProduct.Description = productDTO.Description;
        existingProduct.Price = productDTO.Price;
        existingProduct.Stock = productDTO.Stock;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}