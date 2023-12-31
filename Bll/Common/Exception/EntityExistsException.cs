﻿using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.IdentityModel.Tokens;

namespace Bll.Common.Exception;

public class EntityAlreadyExistsException : System.Exception
{
    public static EntityAlreadyExistsException Create<TEntity>(string? uniqueValueName = null)
    {
        if (uniqueValueName.IsNullOrEmpty())
            return new EntityAlreadyExistsException(typeof(TEntity));
        return new EntityAlreadyExistsException(typeof(TEntity), uniqueValueName!);
    }

    private EntityAlreadyExistsException(Type entityType, string uniqueValueName) :
        base($"Each {entityType.Name} entity must have a unique {uniqueValueName} value. An entity with the given value already exists")
    {
        
    }
    
    private EntityAlreadyExistsException(Type entityType) :
        base($"A(n) {entityType.Name} entity already exists that has some unique constraint on it that does not let you add a new one")
    {
        
    }
}