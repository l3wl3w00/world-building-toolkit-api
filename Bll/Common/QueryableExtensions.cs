using Microsoft.EntityFrameworkCore;

namespace Bll.Common;

public static class QueryableExtensions
{
    public static async Task<TEntity> SingleOrDo<TEntity>(this IQueryable<TEntity> queryable, Action onResultNull)
    {
        var result = await queryable.SingleOrDefaultAsync();
        if (result is null) onResultNull();
        return result!;
    }
}