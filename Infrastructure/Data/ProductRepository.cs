using Core.Entites;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ProductRepository(StoreContext storeContext) : IProductRepository
    {
        public void AddProduct(Product product)
        {
            storeContext.Products.Add(product);
        }

        public async void DeleteProduct(Product product)
        {
            storeContext.Products.Remove(product);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await storeContext.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brands, string? types, string? sort)
        {
            var query = storeContext.Products.AsQueryable();

            if (!string.IsNullOrEmpty(brands))
                query = query.Where(x => x.Brand == brands);
            if (!string.IsNullOrEmpty(types))
                query = query.Where(x => x.Type == types);

            query = sort switch
            {
                "priceAsc" => query.OrderBy(x => x.Price),
                "priceDesc" => query.OrderByDescending(x => x.Price),
                _ => query.OrderBy(x => x.Name)
            };

            return await query.ToListAsync();
        }

        public bool ProductExist(int id)
        {
            return storeContext.Products.Any(p => p.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await storeContext.SaveChangesAsync() > 0;
        }

        public void UpdateProduct(Product product)
        {
            storeContext.Entry(product).State = EntityState.Modified;
        }

        public async Task<IReadOnlyList<string>> GetBrandAsync()
        {
            return await storeContext.Products.Select(p => p.Brand)
                .Distinct().ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetTypesAsync()
        {
            return await storeContext.Products.Select(p => p.Type)
                .Distinct().ToListAsync();
        }
    }
}