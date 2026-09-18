using Ecommerce.server.services.interfaces;
using Ecommerce.server.Utils;
using EcommerceApi.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.server.Controllers
{
    public class OrderController(IOrderService orderService, JwtUtils jwt) : ControllerBase
    {
        [Authorize]
        [HttpGet("checkOut")]
        public async Task<ActionResult<OrderDto>> CheckoutAsync()
        {
            int? usr = jwt.GetIdByToken();

            if (usr == null) return BadRequest("Usuario invaldio");

            OrderDto myOrder = await orderService.CheckoutAsync(usr.Value);
            return Ok(myOrder);
        }

        [Authorize]
        [HttpGet("getMyOrders")]
        public async Task<ActionResult<List<OrderDto>>> GetUserOrdersAsync()
        {
            int? usr = jwt.GetIdByToken();

            if (usr == null) return BadRequest("Usuario invaldio");

            List<OrderDto> myOrders = await orderService.GetUserOrdersAsync(usr.Value);
            return Ok(myOrders ?? []);
        }
    }
}
