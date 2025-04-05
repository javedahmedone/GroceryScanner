using GroceryScanner.Business.Service.Product;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/Product")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("ProductDetails")]
    public async Task<IActionResult> GetProductDetail([FromQuery] string searchItem, [FromQuery] string latitude, [FromQuery] string longitude, [FromQuery] string queryType)
    {
        var result = await _productService.GetProductDetailAsync(searchItem, latitude, longitude, queryType);
        return Ok(result);
    }
}
