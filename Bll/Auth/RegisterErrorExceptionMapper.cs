using Bll.Auth.Exception;
using Bll.Common.Exception;
using Dal.Entities;
using Microsoft.AspNetCore.Identity;
using UserEntity = Dal.Entities.User;
namespace Bll.Auth;

public class RegisterErrorExceptionMapper
{
    public System.Exception ToException(IEnumerable<IdentityError> resultErrors)
    {
        var errorsList = resultErrors.ToList();
        var result = ToException(errorsList.First());
        if (result is null) return new RegisterException(errorsList);
        return result!;
    }

    private System.Exception? ToException(IdentityError error)
    {
        return error.Code switch
        {
            "PasswordRequiresNonAlphanumeric" => new InvalidPasswordException("Password must contain a non alphanumeric character"),
            "PasswordRequiresDigit" => new InvalidPasswordException("Password must contain a digit"),
            "PasswordRequiresUpper" => new InvalidPasswordException("Password must contain an upper case character"),
            "DuplicateUserName" => EntityAlreadyExistsException.Create<UserEntity>("username"),
            "DuplicateEmail" => EntityAlreadyExistsException.Create<UserEntity>("email"),
            _ => null,
        };
    }
}