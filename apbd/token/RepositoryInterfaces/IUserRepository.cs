using System.Security.Claims;
using token.Contracts;
using token.Models;

namespace token.RepositoryInterfaces;

public interface IUserRepository
{
    public  void CheckUserWithMailExists(RegisterUserRequest registerUserRequest);
    public  void CheckPasswordStrongEnough(RegisterUserRequest registerUserRequest);
    public void CheckLoginDetails(LoginRequest request);
    public  Task<Customer> GetCustomer(string refleshToken);
    public Claim[] GetCustomerClaim(Customer customer);
}