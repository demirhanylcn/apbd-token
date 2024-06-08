using token.Contracts;

namespace token.RepositoryInterfaces;

public interface IUserRepository
{
    public  void CheckUserWithMailExists(RegisterUserRequest registerUserRequest);
    public  void CheckPasswordStrongEnough(RegisterUserRequest registerUserRequest);
}