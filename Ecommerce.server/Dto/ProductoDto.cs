namespace Ecommerce.server.Dto
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } 
        public string ImageUrl { get; set; } = null!;
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateUpdateProductoDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int Stock { get; set; }
    }
}
