namespace Bll.Auth.Exception;

public class InvalidPasswordException : System.Exception
{
    public InvalidPasswordException(string message) : base(message)
    {
    }
}