using Ecommerce.server.services.interfaces;
using Ecommerce.server.Utils;
using EcommerceApi.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService orderService, JwtUtils jwt) : ControllerBase
    {
        [Authorize]
        [HttpPost("checkOut")]
        public async Task<ActionResult<OrderDto>> CheckoutAsync(CreateOrderDto request)
        {
            int? usr = jwt.GetIdByToken();

            if (usr == null) return BadRequest("Usuario invaldio");

            OrderDto myOrder = await orderService.CheckoutAsync(usr.Value, request);
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
