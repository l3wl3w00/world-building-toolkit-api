namespace Bll.Exception;
using System;
public class EntityNotFoundException<TEntity> : Exception
{
    public EntityNotFoundException(Guid id) : base($"No {nameof(TEntity)} entity was found with id {id}")
    {
        
    }
    
    public EntityNotFoundException() : base($"No {nameof(TEntity)} entity was found with the given id")
    {
        
    }
}