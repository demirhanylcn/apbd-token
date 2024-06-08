
using System.Text.RegularExpressions;
using token.Context;
using token.Contracts;
using token.Exception;
using token.RepositoryInterfaces;


namespace token.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public  void CheckUserWithMailExists(RegisterUserRequest request)
    {
        var patient = _appDbContext.Users.FirstOrDefault(u => u.Email == request.Email);
        if (patient == null) throw new UserWithMailExistsException(request.Email);
        

    }

    public void CheckPasswordStrongEnough(RegisterUserRequest request)
    {
        var password = request.Password;
        var hasMinimum8Chars = new Regex(@".{8,}");
        var hasUpperCaseLetter = new Regex(@"[A-Z]");
        var hasNumber = new Regex(@"\d");
        var hasSpecialChar = new Regex(@"[!@#$%^&*(),.?""{}|<>]");

        if (!hasUpperCaseLetter.IsMatch(password)
            || !hasMinimum8Chars.IsMatch(password)
            || !hasNumber.IsMatch(password)
            || !hasSpecialChar.IsMatch(password)) throw new WeakPasswordException(password);
    }
}