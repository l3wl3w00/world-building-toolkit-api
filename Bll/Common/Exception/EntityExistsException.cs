using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.IdentityModel.Tokens;

namespace Bll.Common.Exception;

public class EntityAlreadyExistsException : System.Exception
{
    public Type EntityType { get; }

    public static EntityAlreadyExistsException Create<TEntity>(string? uniqueValueName = null)
    {
        if (uniqueValueName.IsNullOrEmpty())
            return new EntityAlreadyExistsException(typeof(TEntity));
        return new EntityAlreadyExistsException(typeof(TEntity), uniqueValueName!);
    }
    
    public static EntityAlreadyExistsException CreateWithMessage<TEntity>(string message)
    {
        return new EntityAlreadyExistsException(
            typeof(TEntity),
            $"A(n) {typeof(TEntity).Name} entity already exists that has some unique constraint on it that does not let you add a new one. " +
            $"message: {message}", 
            0);
    }

    private EntityAlreadyExistsException(Type entityType, string uniqueValueName) :
        base($"Each {entityType.Name} entity must have a unique {uniqueValueName} value. An entity with the given value already exists")
    {
        EntityType = entityType;
    }
    
    private EntityAlreadyExistsException(Type entityType) :
        base($"A(n) {entityType.Name} entity already exists that has some unique constraint on it that does not let you add a new one")
    {
        EntityType = entityType;
    }
    
    private EntityAlreadyExistsException(Type entityType, string message, int _) : base(message)
    {
        EntityType = entityType;
    }

    public bool IsEntitySubTypeOf<T>()
    {
        return EntityType.IsAssignableTo(typeof(T));
    }
}