using Microsoft.AspNetCore.Identity;

namespace Bll.Auth.Exception;

public class RegisterException : System.Exception
{
    public IEnumerable<IdentityError> IdentityErrors { get; }

    public RegisterException(IEnumerable<IdentityError> identityErrors) : base($"An error happened when trying to register")
    {
        IdentityErrors = identityErrors;
    }
}