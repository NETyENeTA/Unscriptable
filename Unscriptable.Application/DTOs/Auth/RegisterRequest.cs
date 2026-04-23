using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Application.DTOs.Auth;

public class RegisterRequest
{

    public string Login { get; set; }
    public string Password { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }



}
