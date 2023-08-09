namespace Bll.Auth.Exception;

public class UserRegisteredThroughOAuthException : System.Exception
{
    public UserRegisteredThroughOAuthException() :
        base("The password was invalid, because the user is most likely registered through a third party oauth service")
    {
        
    }
}