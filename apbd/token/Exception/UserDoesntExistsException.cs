namespace token.Exception;

public class UserDoesntExistsException : System.Exception
{
    public UserDoesntExistsException() : base("check the details you have provided.")
    {
        
    }
}