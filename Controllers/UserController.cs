using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using DTO;
using GuCube.Services;

namespace GuCube.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    [HttpPost("login")]
    [EnableCors("DefaultPolicy")]
    public async Task<IActionResult> Login(
        [FromBody]UserData user,
        [FromServices]IUserService service,
        [FromServices]ISecurityService security
    )
    {
        var loggedUser = await service.GetByLogin(user.login);

        if(loggedUser == null)
            return Unauthorized("Usuário não existe.");

        var password = await security.HashPassword(
            user.password, loggedUser.Salt
        );
        var realPassword = loggedUser.Password;

        if(password != realPassword)
            return Unauthorized("Senha Incorreta.");

        var jwt = await security.GenerateJwt(new {
            id = loggedUser.Id
        });

        return Ok(jwt);
    }

    [HttpPost("signup")]
    [EnableCors("DefaultPolicy")]
    public async Task<IActionResult> Create(
        [FromBody]UserData user,
        [FromServices]IUserService service
    )
    {
        var errors = new List<string>();
        if (user is null || user.login is null)
            errors.Add("É necessário informar um login.");
        if(user.login.Length < 5)
            errors.Add("O Login deve conter ao menos 5 caracteres.");

        if (errors.Count > 0)
            return BadRequest(errors);

        await service.Create(user);
        return Ok();
    }
}
