using DTO;
using GuCube.Model;
using GuCube.Services;
using Microsoft.EntityFrameworkCore;

namespace GuCube.Services;

public class UserService : IUserService
{
    GuCubeContext ctx;
    ISecurityService security;
    public UserService(GuCubeContext ctx, ISecurityService security)
    {
        this.ctx = ctx;
        this.security = security;
    }

    public async Task Create(UserData data)
    {
        User user = new User();
        var salt = await security.GenerateSalt();

        user.Login = data.login;
        user.Password = await security.HashPassword(
            data.password, salt
        );
        user.Name = data.Name;

        this.ctx.Add(user);
        await this.ctx.SaveChangesAsync();
    }

    public async Task<User> GetByLogin(string login)
    {
        var query =
            from u in this.ctx.Users
            where u.Login == login
            select u;

        return await query.FirstOrDefaultAsync();
    }
}