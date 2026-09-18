using Ecommerce.server.Context;
using Ecommerce.server.Dto;
using Ecommerce.server.Mappings;
using Ecommerce.server.Models;
using Ecommerce.server.services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.server.services
{
    public class ProductoService(AppDbContext context) : IProductoService
    {
        public async Task<List<ProductoDto>?> GetProductosAsync(string? term)
        {
            IQueryable<Product> query = context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                var lowerTerm = term.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(lowerTerm));
            }

            List<Product> result = await query.ToListAsync();
            return [.. result.Select(p => p.ToDto())];
        }

        public async Task<ProductoDto?> GetProductoAsync(int id)
        {
            Product? product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

            return product is null ? throw new Exception("No se encontro el producto") : product.ToDto();
        }

        public async Task<ProductoDto?> CreateProductAsync(CreateUpdateProductoDto request)
        {
            bool exists = await context.Products
                .AnyAsync(p => p.Name == request.Name);

            if (exists)
            {
                throw new Exception("El producto ya existe"); 
            }


            Product producto = new()
            {
                Name = request.Name,
                Description = request.Description,
                Stock = request.Stock,
                ImageUrl = request.ImageUrl,
                CreatedAt = DateTime.UtcNow,
                Price = request.Price,
            };


            context.Products.Add(producto);
            await context.SaveChangesAsync();


            return producto.ToDto();
        }

        public async Task<ProductoDto?> UpdateProductAsync(int id, CreateUpdateProductoDto request)
        {
            var producto = await context.Products.FindAsync(id);

            if (producto is null)
            {
                return null;
            }

            bool nameTaken = await context.Products
                .AnyAsync(p => p.Name == request.Name && p.Id != id);

            if (nameTaken)
            {
                return null; 
            }

            producto.Name = request.Name;
            producto.Description = request.Description;
            producto.Stock = request.Stock;
            producto.ImageUrl = request.ImageUrl;
            producto.Price = request.Price;


            context.Products.Entry(producto).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return producto.ToDto();
        }
    }
}
