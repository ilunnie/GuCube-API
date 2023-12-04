namespace GuCube.Services;

using DTO;
using Model;

public interface IUserService
{
    Task Create(UserData data);
    Task<User> GetByLogin(string login);
}