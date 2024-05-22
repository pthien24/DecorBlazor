using Decor_Business.Repository.IRepository;
using Decor_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DecorWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok( await _productRepository.GetAll());
        }

        [HttpGet("{productID}")]
        public async Task<IActionResult> Get(int? productID )
        {
            if (productID ==null || productID == 0)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid ID",
                    StatusCode= StatusCodes.Status400BadRequest
                });
            }
            var product = await _productRepository.Get(productID.Value);
            if (product == null)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid ID",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            return Ok(product);
        }
    }
}
