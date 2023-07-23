using Bll.Auth.Dto;

namespace Bll.Auth.Exception;

public class NotSupportedLoginTypeException : System.Exception
{
    public NotSupportedLoginTypeException(LoginType loginDtoLoginType) : base($"Login type is not supported: {loginDtoLoginType.ToString()}")
    {
        
    }
}