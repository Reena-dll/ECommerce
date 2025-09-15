using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Repositories;

public interface IRepository<T> where T : BaseEntity, new()
{
    Task AddAsync(T entity);
    Task AddListAsync(List<T> entity);
    Task UpdateListAsync(List<T> entity);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool enableTracking = true,
        int currentPage = 1,
        int pageSize = 100
    );
    Task<List<TRes>> GetAllAsync<TRes>(
        Expression<Func<T, TRes>> select,
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool enableTracking = true,
        int currentPage = 1,
        int pageSize = 100
    );
    Task<List<T>> GetAllNoPaginationAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool enableTracking = true
    );
    Task<List<TRes>> GetAllNoPaginationAsync<TRes>(
        Expression<Func<T, TRes>> select,
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool enableTracking = true
    );
    Task<T?> GetAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        bool enableTracking = true
    );
    Task<TRes> GetAsync<TRes>(
        Expression<Func<T, TRes>> select,
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        bool enableTracking = true
    );
    Task<int> SumAsync(Expression<Func<T, bool>>? predicate, Expression<Func<T, int>> selector);
}
