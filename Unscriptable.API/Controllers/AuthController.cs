using Microsoft.AspNetCore.Mvc;
using Unscriptable.Application.DTOs.Auth;
using Unscriptable.Application.Interfaces;
using Unscriptable.Infrastructure.Services;

namespace Unscriptable.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = await authService.RegisterAsync(
                request.Login,
                request.Password,
                request.Email,
                request.FirstName,
                request.LastName);

            return Ok(new { user.Id, user.Login });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login-cookie")]
    public async Task<IActionResult> LoginCookie([FromBody] LoginRequest request)
    {
        var success = await authService.LoginCookieAsync(request.Login, request.Password);

        if (!success)
            return Unauthorized("Неверный логин или пароль");

        return Ok(new { message = "Вход через Cookie выполнен успешно" });
    }

    [HttpPost("login-jwt")]
    public async Task<IActionResult> LoginJwt([FromBody] LoginRequest request)
    {
        var token = await authService.LoginJwtAsync(request.Login, request.Password);

        if (string.IsNullOrEmpty(token))
            return Unauthorized("Неверный логин или пароль");

        return Ok(new { token });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await authService.LogoutAsync();
        return Ok(new { message = "Вы успешно вышли из системы" });
    }
}


//public record RegisterRequest(string Login, string Password, string Email, string FirstName, string LastName);
//public record LoginRequest(string Login, string Password);
