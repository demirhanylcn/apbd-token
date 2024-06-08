
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using token.Context;
using token.Contracts;
using token.Exception;
using token.Models;
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
        var customer = _appDbContext.Customers.Any(u => u.Email == request.Email);
        if (customer) throw new UserWithMailExistsException(request.Email);
        

    }

    public void CheckPasswordStrongEnough(RegisterUserRequest request)
    {
        var password = request.Password;
        var hasMinimum8Chars = new Regex(@".{16,}");
        var hasUpperCaseLetter = new Regex(@"[A-Z]");
        var hasNumber = new Regex(@"\d");
        var hasSpecialChar = new Regex(@"[!@#$%^&*(),.?""{}|<>]");

        if (!hasUpperCaseLetter.IsMatch(password)
            || !hasMinimum8Chars.IsMatch(password)
            || !hasNumber.IsMatch(password)
            || !hasSpecialChar.IsMatch(password)) throw new WeakPasswordException(password);
    }

    public void CheckLoginDetails(LoginRequest request)
    {
        var customer = _appDbContext.Customers.Any(c => c.Email == request.Email);
        if (!customer) throw new UserDoesntExistsException();
    }

    public async Task<Customer> GetCustomer(string refleshToken)
    {
        var customer = await _appDbContext.Customers.FirstOrDefaultAsync(c => c.RefreshToken == refleshToken);
        return customer;
    }

    public Claim[] GetCustomerClaim(Customer customer)
    {
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Convert.ToString(customer.Id)),
            new Claim(ClaimTypes.Name, $"{customer.FirstName} {customer.LastName}"),
            new Claim(ClaimTypes.Email, customer.Email),
            new Claim(ClaimTypes.Role, "Customer")
        };

        return userClaims;
    }
}