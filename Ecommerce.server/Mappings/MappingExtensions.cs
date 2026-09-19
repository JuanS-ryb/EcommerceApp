using Ecommerce.server.Dto;
using Ecommerce.server.Models;
using EcommerceApi.DTOs.Orders;

namespace Ecommerce.server.Mappings;

public static class MappingExtensions
{
    public static UserDto ToDto(this User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
    };

    public static ProductoDto ToDto(this Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description ?? "",
        Price = product.Price,
        ImageUrl = product.ImageUrl ?? "",
        Stock = product.Stock,
        CreatedAt = product.CreatedAt,
    };

    public static CartItemDto ToDto(this CartItem item) => new()
    {
        Quantity = item.Quantity,
        ProductId = item.ProductId,
        Product = item.Product?.ToDto() ,   // reutiliza el mapeo anterior
    };

    public static OrderItemDto ToDto(this OrderItem item) => new()
    {
        Quantity = item.Quantity,
        ProductId = item.ProductId,
        ProductName = item.Product?.Name ?? string.Empty,
    };

    public static OrderDto ToDto(this Order order) => new()
    {
        Id = order.Id,
        Address = order.Address,
        PaymentMethod = order.PaymentMethod,
        OrderDate = order.OrderDate,
        Status = order.Status,
        Total = order.Total,
        Items = order.OrderItems?.Select(i => i.ToDto()).ToList() ?? [],
    };
}