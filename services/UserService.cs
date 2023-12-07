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
        user.Salt = salt;

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

    public async Task<User> GetById(int id)
    {
        var query = 
            from u in this.ctx.Users
            where u.Id == id
            select u;

        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<Store>> GetManaged(int id)
{
    var query =
        from storeManager in this.ctx.StoreManagers
        where storeManager.UserId == id
        select storeManager.StoreId;

    var storeIds = await query.ToListAsync();

    var storesQuery =
        from store in this.ctx.Stores
        where storeIds.Contains(store.Id)
        select store;
    
    return await storesQuery.ToListAsync();
}
}