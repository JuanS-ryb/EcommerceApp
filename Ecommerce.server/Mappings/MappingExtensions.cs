using Ecommerce.server.Dto;
using Ecommerce.server.Models;

namespace EcommerceApi.Mappings;

public static class MappingExtensions
{
    public static UserDto ToDto(this User user) => new()
    {
        Name = user.Name,
        Email = user.Email,
    };

    public static ProductoDto ToDto(this Product product) => new()
    {
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        ImageUrl = product.ImageUrl,
        Stock = product.Stock,
        CreatedAt = product.CreatedAt,
    };

    public static CartItemDto ToDto(this CartItem item) => new()
    {
        Quantity = item.Quantity,
        ProductId = item.ProductId,
        Product = item.Product?.ToDto(),   // reutiliza el mapeo anterior
    };

    //public static OrderItemDto ToDto(this OrderItem item) => new()
    //{
    //    Id = item.Id,
    //    Quantity = item.Quantity,
    //    Price = item.Price,
    //    ProductId = item.ProductId,
    //    ProductName = item.Product?.Name ?? string.Empty,
    //};

    //public static OrderDto ToDto(this Order order) => new()
    //{
    //    Id = order.Id,
    //    UserId = order.UserId,
    //    CreatedAt = order.CreatedAt,
    //    Total = order.Total,
    //    Items = order.OrderItems?.Select(i => i.ToDto()).ToList() ?? new List<OrderItemDto>(),
    //};
}