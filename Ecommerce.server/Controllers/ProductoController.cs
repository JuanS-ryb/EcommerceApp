using Ecommerce.server.Dto;
using Ecommerce.server.services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController(IProductoService productoService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ProductoDto>>> GetProductos([FromQuery] string? term)
        {
            List<ProductoDto> productos = await productoService.GetProductosAsync(term) ?? [];

            return Ok(productos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<List<ProductoDto>>> GetProducto(string id)
        {
            if (!int.TryParse(id, out int prod) || prod <= 0)
            {
                return BadRequest(new { message = $"El id de producto '{id}' no es válido." });
            }

            ProductoDto? producto = await productoService.GetProductoAsync(prod);

            if (producto is null) return NotFound("No se pudo encontrar el producto con el id " + id);

            return Ok(producto);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductoDto>> Update(string id, CreateUpdateProductoDto request)
        {
            if (!int.TryParse(id, out int prod) || prod <= 0)
            {
                return BadRequest(new { message = $"El id de producto '{id}' no es válido." });
            }
            ProductoDto? producto = await productoService.UpdateProductAsync(prod, request);

            if (producto is null)
                return NotFound();

            return Ok(producto);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProductoDto>> Create(CreateUpdateProductoDto request)
        {
            try
            {
                ProductoDto? producto = await productoService.CreateProductAsync(request);
                if (producto is null) return NotFound();

                return Ok(producto);
            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
    }

}