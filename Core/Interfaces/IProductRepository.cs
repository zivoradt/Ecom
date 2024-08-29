using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(string? brands, string? types, string? sort);

        Task<IReadOnlyList<string>> GetBrandAsync();

        Task<IReadOnlyList<string>> GetTypesAsync();

        Task<Product?> GetByIdAsync(int id);

        void AddProduct(Product product);

        void UpdateProduct(Product product);

        void DeleteProduct(Product product);

        bool ProductExist(int id);

        Task<bool> SaveChangesAsync();
    }
}