using Microsoft.AspNetCore.Identity;

namespace Bll.Auth.Exception;

public class RegisterException : System.Exception
{
    public IEnumerable<IdentityError> IdentityErrors { get; }

    public RegisterException(IEnumerable<IdentityError> identityErrors)
    {
        IdentityErrors = identityErrors;
    }

    public override string Message => "Errors when trying to register: " + string.Join(", ", IdentityErrors.Select(e => e.Code));
}