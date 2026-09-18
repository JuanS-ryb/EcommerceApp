using Ecommerce.server.Dto;
using Ecommerce.server.Models;

namespace Ecommerce.server.services.interfaces
{
    public interface IProductoService
    {
        Task<List<Product>?> GetProductosAsync(string? term);
        Task<Product?> GetProductoAsync(int id);
        Task<Product?> CreateProductAsync(ProductoDto request);
        Task<Product?> UpdateProductAsync(int id, ProductoDto request);
    }

    public interface IAuthService
    {
        Task<string?> LoginAsync(UserDto request);
        Task<User?> RegisterAsync(UserDto request);
    }

    public interface ICartService
    {
        Task<CartDto> GetCartAsync(int userId);
        Task<CartDto> AddItemAsync(int userId, AddToCartRequestDto request);
        Task<CartDto> UpdateItemAsync(int userId, int productId, UpdateCartItemRequestDto request);
        Task<CartDto> RemoveItemAsync(int userId, int productId);
    }
}
