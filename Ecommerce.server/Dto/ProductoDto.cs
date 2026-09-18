using Ecommerce.server.Models;

namespace Ecommerce.server.Dto
{
    public class ProductoDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } 
        public string ImageUrl { get; set; } = null!;
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
