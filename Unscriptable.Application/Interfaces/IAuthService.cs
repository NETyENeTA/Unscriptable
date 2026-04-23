using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unscriptable.Domain.Entitties;

namespace Unscriptable.Application.Interfaces;

public interface IAuthService
{

    Task<User> RegisterAsync(string login, string password, string email, string firstName, string lastName);
    Task<bool> LoginCookieAsync(string login, string password);
    Task<string?> LoginJwtAsync(string login, string password);
    Task LogoutAsync();
}
