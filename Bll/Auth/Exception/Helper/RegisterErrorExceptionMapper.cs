using Bll.Common.Exception;
using Bll.Common.Option;
using Microsoft.AspNetCore.Identity;
using UserEntity = Dal.Entities.User;
namespace Bll.Auth.Exception.Helper;

public class RegisterErrorExceptionMapper
{
    public System.Exception ToException(IEnumerable<IdentityError> resultErrors)
    {
        var errorsList = resultErrors.ToList();
        var result = ToException(errorsList.First());
        return result.MapIfNull(() => new RegisterException(errorsList));
    }

    private Option<System.Exception> ToException(IdentityError error)
    {
        return error.Code switch
        {
            "PasswordRequiresNonAlphanumeric" => 
                (new InvalidPasswordException("Password must contain a non alphanumeric character") as System.Exception)
                .ToOption(),
            "PasswordRequiresDigit" => 
                (new InvalidPasswordException("Password must contain a digit") as System.Exception)
                .ToOption(),
            "PasswordRequiresUpper" => 
                (new InvalidPasswordException("Password must contain an upper case character") as System.Exception)
                .ToOption(),
            "DuplicateUserName" => 
                (EntityAlreadyExistsException.Create<UserEntity>("username") as System.Exception)
                .ToOption(),
            "DuplicateEmail" => 
                (EntityAlreadyExistsException.Create<UserEntity>("email") as System.Exception)
                .ToOption(),
            _ => Option<System.Exception>.None,
        };
    }
}