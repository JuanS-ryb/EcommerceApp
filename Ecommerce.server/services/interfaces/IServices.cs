using Ecommerce.server.Dto;
using Ecommerce.server.Models;
using EcommerceApi.DTOs.Orders;

namespace Ecommerce.server.services.interfaces
{
    public interface IProductoService
    {
        Task<List<ProductoDto>?> GetProductosAsync(string? term);
        Task<ProductoDto?> GetProductoAsync(int id);
        Task<ProductoDto?> CreateProductAsync(CreateUpdateProductoDto request);
        Task<ProductoDto?> UpdateProductAsync(int id, CreateUpdateProductoDto request);
    }

    public interface IAuthService
    {
        Task<AuthDto?> LoginAsync(UserDto request);
        Task<User?> RegisterAsync(UserDto request);
        Task<User?> GetMyUser();
    }

    public interface ICartService
    {
        Task<CartDto> GetCartAsync(int userId);
        Task<CartDto> AddItemAsync(int userId, AddToCartRequestDto request);
        Task<CartDto> UpdateItemAsync(int userId, int productId, UpdateCartItemRequestDto request);
        Task<CartDto> RemoveItemAsync(int userId, int productId);
    }

    public interface IOrderService
    {
        Task<OrderDto> CheckoutAsync(int userId, CreateOrderDto request);
        Task<List<OrderDto>> GetUserOrdersAsync(int userId);
    }

    public interface ISalesGoalService
    {
        Task<SalesGoalDto> GetCurrentAsync(int userId);
        Task<SalesGoalDto> UpdateExecutedAsync(int userId, UpdateExecutedRequestDto request);
    }
}
