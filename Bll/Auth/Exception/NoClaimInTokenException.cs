namespace Bll.Auth.Exception;

public class NoClaimInTokenException : System.Exception
{
    public NoClaimInTokenException(string claimName) : base($"There is no {claimName} claim in the jwt token, so the user cannot be identified")
    {
        
    }
}