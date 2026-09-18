using Ecommerce.server.Dto;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.server.Dto;

public class CartItemDto
{
    public int ProductId { get; set; }
    public ProductoDto Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Subtotal => Product.Price * Quantity;
}

public class CartDto
{
    public List<CartItemDto> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Subtotal);
}

public class AddToCartRequestDto
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
    public int Quantity { get; set; } = 1;
}

public class UpdateCartItemRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
    public int Quantity { get; set; }
}
