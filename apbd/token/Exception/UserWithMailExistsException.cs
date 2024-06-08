namespace token.Exception;

public class UserWithMailExistsException : System.Exception
{
    public UserWithMailExistsException(string email) : base($"user with mail {email} exists.")
    {
        
    }
}