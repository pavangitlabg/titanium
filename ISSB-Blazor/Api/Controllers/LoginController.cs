using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _config;

    public LoginController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login([FromBody] UserLogin userLogin)
    {
        var user = Authenticate(userLogin);

        if (user.Result != null)
        {
            var token = Generate(user);
            return Ok(token);
        }

        return NotFound("User not found");
    }

    private string Generate(Task<UserModel> user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Result.CompanyName),
            new Claim(ClaimTypes.Email, user.Result.Email),
            new Claim(ClaimTypes.GivenName, user.Result.CompanyName),
            new Claim(ClaimTypes.Surname, user.Result.LastName),
            new Claim(ClaimTypes.PrimarySid, user.Result._id),
            new Claim(ClaimTypes.Role, "API_User") //ToDo Add User Role
        };

        var test = "DhftOS5uphK3vmCJQrexST1RsyjZBjXWRgJMFPU4";
        //var token = new JwtSecurityToken(_config["Jwt:Issuer"],
        var token = new JwtSecurityToken(test,
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddDays(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<UserModel> Authenticate(UserLogin userLogin)
    {
        var userModel = new UserModel { Email = userLogin.Username, Password = userLogin.Password };
        var authSrv = new UserServices();
        var currentUser = await authSrv.LoginUser(userModel.Email, userModel.Password);
//ToDo Check that the account has no locks

        if (currentUser.Email != null && currentUser.IsAPI) return currentUser;

        return null;
    }
}