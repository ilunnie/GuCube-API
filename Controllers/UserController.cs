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
            return NotFound(new { message = "Usuário não existe" });

        var password = await security.HashPassword(
            user.password, loggedUser.Salt
        );
        var realPassword = loggedUser.Password;

        if(password != realPassword)
            return Unauthorized(new { message = "Senha Incorreta." });
        var jwt = await security.GenerateJwt(new {
            id = loggedUser.Id
        });

        return Ok(new { token = jwt });
    }

    [HttpPost("signup")]
    [EnableCors("DefaultPolicy")]
    public async Task<IActionResult> Create(
        [FromBody]UserData user,
        [FromServices]IUserService service
    )
    {
        var errors = new List<string>();
        var loginUser = await service.GetByLogin(user.login);
        if (loginUser != null)
            errors.Add("O login já está em uso.");
        if (user is null || user.login is null)
            errors.Add("É necessário informar um login.");
        if (user?.login?.Length < 5)
            errors.Add("O Login deve conter ao menos 5 caracteres.");

        if (errors.Count > 0)
            return BadRequest(errors);

        await service.Create(user!);
        return Ok();
    }

    [HttpPost("managed")]
    [EnableCors("DefaultPolicy")]
    public async Task<IActionResult> GetManaged(
        [FromBody]JwtData jwt,
        [FromServices]IUserService service,
        [FromServices]ISecurityService security
    )
    {
        var jwtPayload = await security.ValidadeJwt<JwtPayload>(jwt.token);
        var stores = await service.GetManaged(jwtPayload.id);
        return Ok(stores);
    }
}
