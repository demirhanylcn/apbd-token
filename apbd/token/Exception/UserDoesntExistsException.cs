namespace token.Exception;

public class UserDoesntExistsException : System.Exception
{
    public UserDoesntExistsException() : base("check the login details you have provided.")
    {
        
    }
}