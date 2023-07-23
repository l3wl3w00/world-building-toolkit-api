namespace Bll.Auth.Exception;

public class LoginException : System.Exception
{
    public LoginException() : base("Failed to log in")
    {
        
    }
}