using Bll.Auth.Exception;
using Bll.Common.Exception;
using Dal.Entities;
using Microsoft.AspNetCore.Identity;

namespace Bll.Auth;

public class RegisterErrorExceptionMapper
{
    public System.Exception ToException(IEnumerable<IdentityError> resultErrors)
    {
        var result = ToException(resultErrors.First());
        if (result is null) return new RegisterException(resultErrors);
        return result!;
    }

    private System.Exception? ToException(IdentityError error)
    {
        return error.Code switch
        {
            "PasswordRequiresNonAlphanumeric" => new InvalidPasswordException("Password must contain a non alphanumeric character"),
            "PasswordRequiresDigit" => new InvalidPasswordException("Password must contain a digit"),
            "PasswordRequiresUpper" => new InvalidPasswordException("Password must contain an upper case character"),
            "DuplicateUserName" => EntityAlreadyExistsException.Create<User>("username"),
            "DuplicateEmail" => EntityAlreadyExistsException.Create<User>("email"),
            _ => null,
        };
    }
}