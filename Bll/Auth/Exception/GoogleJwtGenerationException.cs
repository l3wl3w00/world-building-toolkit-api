namespace Bll.Auth.Exception;

public class GoogleJwtGenerationException : System.Exception
{
    public GoogleJwtGenerationException(): base("Something went wrong when trying to generate jwt token for google account")
    {
    }
}