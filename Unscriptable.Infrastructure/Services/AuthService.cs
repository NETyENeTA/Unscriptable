using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Unscriptable.Application.Interfaces;
using Unscriptable.Domain.Entitties;
using Unscriptable.Domain.Enums;
using Unscriptable.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;

namespace Unscriptable.Infrastructure.Services;

public class AuthService(
    AppDbContext context,
    JwtService jwtService,
    CookieService cookieService,
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    // 1. Регистрация
    public async Task<User> RegisterAsync(string login, string password, string email, string firstName, string lastName)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            Login = login,
            Passwordhash = passwordHash,
            Email = email,
            Firstname = firstName,
            Lastname = lastName,
            Role = UserRole.Student.ToString() // По умолчанию студент
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Если это студент — создаем ему пустой профиль в таблице Students
        if (user.Role == UserRole.Student.ToString())
        {
            context.Students.Add(new Student { Userid = user.Id });
            await context.SaveChangesAsync();
        }

        return user;
    }

    // 2. Логин через Cookie
    public async Task<bool> LoginCookieAsync(string login, string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Login == login);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Passwordhash))
            return false;

        var principal = cookieService.CreatePrincipal(user);

        await httpContextAccessor.HttpContext!.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return true;
    }

    // 3. Логин через JWT
    public async Task<string?> LoginJwtAsync(string login, string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Login == login);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Passwordhash))
            return null;

        return jwtService.GenerateToken(user);
    }

    // 4. Логаут из Cookie
    public async Task LogoutAsync()
    {
        await httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}