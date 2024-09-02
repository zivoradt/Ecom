using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> ListAllAsync();

        Task<T?> GetEntityWithSpec(ISpecification<T> specification);

        Task<IReadOnlyList<T?>> ListAsync(ISpecification<T> specification);

        Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> specification);

        Task<IReadOnlyList<TResult?>> ListAsync<TResult>(ISpecification<T, TResult> specification);

        void Add(T entity);

        void Update(T entity);

        void Remove(T entity);

        Task<bool> SaveAllAsync();

        Task<int> CountAsync(ISpecification<T> specification);

        bool Exist(int id);
    }
}