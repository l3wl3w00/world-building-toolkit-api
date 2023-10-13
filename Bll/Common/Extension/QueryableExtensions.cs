using System.Linq.Expressions;
using Bll.Common.Option;
using Microsoft.EntityFrameworkCore;

namespace Bll.Common.Extension;

public static class QueryableExtensions
{
    public static async Task<TEntity> SingleOrDo<TEntity>(this IQueryable<TEntity> queryable, Action onResultNull)
    {
        var result = await queryable.SingleOrDefaultAsync();
        if (result is null) onResultNull();
        return result!;
    }
    
    public static async Task<Option<TEntity>> FirstOrOptionAsync<TEntity>(this IQueryable<TEntity> queryable)
    {
        var result = await queryable.FirstOrDefaultAsync();
        return result.ToOption();
    }
    
    public static async Task<Option<TEntity>> SingleOrOptionAsync<TEntity>(this IQueryable<TEntity> queryable)
    {
        var result = await queryable.SingleOrDefaultAsync();
        return result.ToOption();
    }
    
    public static async Task<Option<TEntity>> SingleOrOptionAsync<TEntity>(
        this IQueryable<TEntity> queryable,
        Expression<Func<TEntity,bool>> filter)
    {
        var result = await queryable.SingleOrDefaultAsync(filter);
        return result.ToOption();
    }
}