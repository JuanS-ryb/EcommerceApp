using Ecommerce.server.Dto;
using Ecommerce.server.Models;
using Ecommerce.server.services.auth;
using Ecommerce.server.services.producto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Ecommerce.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController(IProductoService productoService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProductos([FromQuery] string? term)
        {
            var productos = await productoService.GetProductosAsync(term);

            return Ok(productos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<List<Product>>> GetProducto(int id)
        {
            Product? producto = await productoService.GetProductoAsync(id);

            if (producto is null) return NotFound("No se pudo encontrar el producto con el id " + id);

            return Ok(producto);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Update(int id, ProductoDto request)
        {
            var producto = await productoService.UpdateProductAsync(id, request);

            if (producto is null)
                return NotFound();

            return Ok(producto);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Product>> Create(ProductoDto request)
        {
            try
            {
                var producto = await productoService.CreateProductAsync(request);
                if (producto is null) return NotFound();

                return Ok(producto);
            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
    }

}