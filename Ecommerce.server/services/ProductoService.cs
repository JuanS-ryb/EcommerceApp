using Ecommerce.server.Context;
using Ecommerce.server.Dto;
using Ecommerce.server.Models;
using Microsoft.EntityFrameworkCore;
using Ecommerce.server.services.interfaces;

namespace Ecommerce.server.services
{
    public class ProductoService(AppDbContext context) : IProductoService
    {
        public async Task<List<Product>?> GetProductosAsync(string? term)
        {
            IQueryable<Product> query = context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                var lowerTerm = term.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(lowerTerm));
            }

            List<Product> result = await query.ToListAsync();
            return result;
        }

        public async Task<Product?> GetProductoAsync(int id)
        {
            Product? product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }

        public async Task<Product?> CreateProductAsync(ProductoDto request)
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


            return producto;
        }

        public async Task<Product?> UpdateProductAsync(int id, ProductoDto request)
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

            return producto;
        }
    }
}
