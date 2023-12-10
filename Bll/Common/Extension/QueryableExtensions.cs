using System.Linq.Expressions;
using AutoMapper;
using Bll.Common.Option_;
using Bll.Common.Result_;
using Dal.Entities;
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
    
    public static async Task<Result<TEntity>> SingleOrError<TEntity, TError>(this IQueryable<TEntity> queryable, TError error) 
        where TError : System.Exception
    {
        try
        {
            var result = await queryable.SingleOrDefaultAsync();
            if (result == null)
            {
                return Result.Err<TEntity>(error);
            }

            return Result.Ok<TEntity>(result);
        }
        catch (System.Exception e)
        {
            return Result.Err<TEntity>(e);
        }
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
    
    public static TOther Map<TOther>(this IModel entity, IMapper mapper)
    {
        return mapper.Map<TOther>(entity);
    }
}