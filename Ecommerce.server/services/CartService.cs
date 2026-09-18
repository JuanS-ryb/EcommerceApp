using Ecommerce.server.Context;
using Ecommerce.server.Dto;
using Ecommerce.server.Models;
using Ecommerce.server.services.interfaces;
using EcommerceApi.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Ecommerce.server.Mappings;

namespace Ecommerce.server.services
{
    public class CartService(AppDbContext context) : ICartService 
    {
        public async Task<CartDto> GetCartAsync(int userId)
        {
            var items = await context.CartItems
                .Include(ci => ci.Product)
                .AsNoTracking()
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            return new CartDto
            {
                Items = [.. items.Select(i => i.ToDto())]
            };
        }

        public async Task<CartDto> AddItemAsync(int userId, AddToCartRequestDto request)
        {
            var product = await context.Products.FindAsync(request.ProductId)
                ?? throw new NotFoundException($"No se encontró el producto con id {request.ProductId}.");

            if (product.Stock < request.Quantity)
            {
                throw new BadRequestException($"No hay suficiente stock de '{product.Name}'.");
            }

            var existing = await context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == request.ProductId);

            if (existing is not null)
            {
                existing.Quantity += request.Quantity;
            }
            else
            {
                context.CartItems.Add(new CartItem
                {
                    UserId = userId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await context.SaveChangesAsync();
            return await GetCartAsync(userId);
        }

        public async Task<CartDto> UpdateItemAsync(int userId, int productId, UpdateCartItemRequestDto request)
        {
            var item = await context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId)
                ?? throw new NotFoundException("El producto no está en el carrito.");

            item.Quantity = request.Quantity;
            await context.SaveChangesAsync();

            return await GetCartAsync(userId);
        }

        public async Task<CartDto> RemoveItemAsync(int userId, int productId)
        {
            var item = await context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId)
                ?? throw new NotFoundException("El producto no está en el carrito.");

            context.CartItems.Remove(item);
            await context.SaveChangesAsync();

            return await GetCartAsync(userId);
        }
    }
}
