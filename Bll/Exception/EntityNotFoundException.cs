using Microsoft.Identity.Client;

namespace Bll.Exception;
using System;
public class EntityNotFoundException : Exception
{
    public static EntityNotFoundException Throw<TEntity>(Guid? id = null)
    {
        if (id is null)
            throw new EntityNotFoundException(typeof(TEntity));
        throw new EntityNotFoundException(typeof(TEntity), id.Value);
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