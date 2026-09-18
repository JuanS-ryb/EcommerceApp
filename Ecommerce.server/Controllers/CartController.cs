using Ecommerce.server.Dto;
using Ecommerce.server.Models;
using Ecommerce.server.services;
using Ecommerce.server.services.interfaces;
using Ecommerce.server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Ecommerce.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ICartService cartService, JwtUtils jwt): ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart()
        {
            int? usr = jwt.GetIdByToken();

            if (usr == null) return BadRequest("Usuario invaldio");

            CartDto myCart = await cartService.GetCartAsync(usr.Value);
            return Ok(myCart);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CartDto>> AddItem(AddToCartRequestDto request)
        {
            int? usr = jwt.GetIdByToken();

            if (usr == null) return BadRequest("Usuario invaldio");

            CartDto myCart = await cartService.AddItemAsync(usr.Value, request);
            return Ok(myCart);
        }

        [Authorize]
        [HttpPut("{prodId}")]
        public async Task<ActionResult<CartDto>> UpdateItemAsync(string prodId, UpdateCartItemRequestDto request)
        {
            int? usr = jwt.GetIdByToken();
            if (!int.TryParse(prodId, out int prod) || prod <= 0)
            {
                return BadRequest(new { message = $"El id de producto '{prodId}' no es válido." });
            }

            if (usr == null) return BadRequest("Usuario invaldio");

            CartDto myCart = await cartService.UpdateItemAsync(usr.Value, prod, request);
            return Ok(myCart);
        }


        [Authorize]
        [HttpDelete("{prodId}")]
        public async Task<ActionResult<CartDto>> DeleteProduct(string prodId)
        {
            int? usr = jwt.GetIdByToken();
            if (!int.TryParse(prodId, out int prod) || prod <= 0)
            {
                return BadRequest(new { message = $"El id de producto '{prodId}' no es válido." });
            }

            if (usr == null) return BadRequest("Usuario invaldio");

            CartDto MyCart = await cartService.RemoveItemAsync(usr.Value, prod);
            return Ok(MyCart);
        }
    }
}