using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CRM_Helper;
using CRM_API.Data;
using CRM_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly WebApiDBContext _context;
        public AuthentificationController(WebApiDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(Account account)
        {
            UserAccount? userAcc = new() { Login = account.Login, Password = account.Password };
            userAcc = await _context.UserAccounts.
                SingleOrDefaultAsync(a => a.Login == userAcc.Login && a.Password == userAcc.Password);

            if (userAcc == null)
                return NotFound("Неверны логин или пароль.");

            var claims = new List<Claim> { new(ClaimTypes.Name, account.Login) };

            var jwt = new JwtSecurityToken(
                issuer: AuthentificationOptions.ISSUER,
                audience: AuthentificationOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(20)),
                signingCredentials: new SigningCredentials(AuthentificationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", token,
            new CookieOptions
            {
                MaxAge = TimeSpan.FromHours(12)
            });

            return NoContent();
        }
    }
}
