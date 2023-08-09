namespace Bll.Auth.Exception;

public class UserNotLoggedInException : System.Exception
{
    public UserNotLoggedInException() : base("The user is not logged in, and tried to do an operation that requires authentication")
    {
        
    }
}