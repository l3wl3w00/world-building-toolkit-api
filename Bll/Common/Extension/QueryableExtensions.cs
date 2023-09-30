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
        return Option<TEntity>.FromNullable(result);
    }
}