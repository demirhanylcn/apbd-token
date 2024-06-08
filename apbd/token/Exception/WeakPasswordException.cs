namespace token.Exception;

public class WeakPasswordException : System.Exception
{
    public WeakPasswordException(string password) : base($"the password provided {password} is really weak.")
    {
        
    }
}