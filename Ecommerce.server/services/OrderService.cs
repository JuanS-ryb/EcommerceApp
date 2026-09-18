using Ecommerce.server.Context;
using Ecommerce.server.Dto;
using Ecommerce.server.Mappings;
using Ecommerce.server.Models;
using Ecommerce.server.services.interfaces;
using EcommerceApi.Common.Exceptions; // ajusta si moviste tus excepciones a Ecommerce.server.*
using EcommerceApi.DTOs.Orders;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.server.services;

public class OrderService(AppDbContext context) : IOrderService
{
    private readonly AppDbContext _context = context;

    public async Task<OrderDto> CheckoutAsync(int userId)
    {
        var cartItems = await _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.UserId == userId)
            .ToListAsync();

        if (cartItems.Count == 0)
        {
            throw new BadRequestException("El carrito está vacío.");
        }

        // Validar stock antes de confirmar, para no dejar la orden a medias.
        var withoutStock = cartItems.FirstOrDefault(ci => ci.Product.Stock < ci.Quantity);
        if (withoutStock is not null)
        {
            throw new BadRequestException($"No hay suficiente stock de '{withoutStock.Product.Name}'.");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            Status = "Completed",
            Total = cartItems.Sum(ci => ci.Product.Price * ci.Quantity),
            OrderItems = [.. cartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.Price // se congela el precio al momento de la compra
            })]
        };

        foreach (var ci in cartItems)
        {
            ci.Product.Stock -= ci.Quantity;
        }

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cartItems);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        // 'saved' es UN pedido (Order), no una lista -> se mapea directo, sin .Select()
        var saved = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .AsNoTracking()
            .FirstAsync(o => o.Id == order.Id);

        return saved.ToDto();
    }

    public async Task<List<OrderDto>> GetUserOrdersAsync(int userId)
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .AsNoTracking()
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        // orders SÍ es una lista -> aquí el .Select() es correcto
        return [.. orders.Select(o => o.ToDto())];
    }
}