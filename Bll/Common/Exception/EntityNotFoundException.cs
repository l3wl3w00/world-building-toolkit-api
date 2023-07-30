using Microsoft.Identity.Client;

namespace Bll.Common.Exception;
using System;

public class EntityNotFoundException : Exception
{
    public static void Throw<TEntity>(Guid? id = null)
    {
        if (id is null)
            throw new EntityNotFoundException(typeof(TEntity));
        throw new EntityNotFoundException(typeof(TEntity), id.Value);
    }

    public static void Throw<TEntity>(string searchValueName, object searchValue)
    {
        throw new EntityNotFoundException(
            $"No {typeof(TEntity).Name} entity was found with {searchValueName} {searchValue.ToString()}.");
    }
    private EntityNotFoundException(Type entityType, Guid id) : base($"No {entityType.Name} entity was found with id {id}")
    {
        
    }
    
    private EntityNotFoundException(Type entityType) : base($"No {entityType.Name} entity was found with the given id")
    {
        
    }

    public EntityNotFoundException(string message) : base(message)
    {
        
    }
}