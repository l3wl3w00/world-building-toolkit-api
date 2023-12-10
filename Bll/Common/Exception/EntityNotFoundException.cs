using Bll.Common.Option_;
using Bll.Common.Result_;
using Microsoft.Identity.Client;

namespace Bll.Common.Exception;
using System;

public class EntityNotFoundException : Exception
{
    public Type EntityType { get; }

    public static EntityNotFoundException Create<TEntity>(Guid? id = null)
    {
        if (id is null)
            return new EntityNotFoundException(typeof(TEntity));
        return new EntityNotFoundException(typeof(TEntity), id.Value);
    }
    
    public static Result<T> CreateResult<T, TEntity>(Guid? id = null)
    {
        return Create<TEntity>(id).ToErrorResult<T>();
    }

    public static EntityNotFoundException Create<TEntity>(string searchValueName, object searchValue)
    {
        return new EntityNotFoundException(typeof(TEntity),
            $"No {typeof(TEntity).Name} entity was found with {searchValueName} {searchValue.ToString()}.");
    }
    private EntityNotFoundException(Type entityType, Guid id) : base($"No {entityType.Name} entity was found with id {id}")
    {
        EntityType = entityType;
    }
    
    private EntityNotFoundException(Type entityType) : base($"No {entityType.Name} entity was found with the given id")
    {
        EntityType = entityType;
    }

    public EntityNotFoundException(Type entityType, string message) : base(message)
    {
        EntityType = entityType;
    }
    
    public bool IsEntitySubTypeOf<T>()
    {
        return EntityType.IsAssignableTo(typeof(T));
    }
}

public static class EntityNotFoundExceptionUtils 
{
    public static Result<T> ValueOrEntityNotFoundException<T>(this Option<T> option, Guid? id = null)
    {
        if (option.HasValue) return Result.Ok(option.Value);
        return Result.Err<T>(EntityNotFoundException.Create<T>(id));
    }
}