using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using CRM_Helper;
using CRM_APIRequests;
using System;
using System.Net;
using System.Security.Claims;

namespace  Web_Client.Controllers
{
    public class AuthController : Controller
    {
        private readonly SkillProfiWebClient _SPClient = new();

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Redirect("~/");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string ReturnUrl, Account account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }

            try
            {
                await _SPClient.Accounts.LoginAsync(account);
            }
            catch (HttpRequestException ex)
            {
                var sc = ex.StatusCode;

                string mes;

                mes = sc switch
                {
                    HttpStatusCode.BadRequest => "Bad format of some field",
                    HttpStatusCode.NotFound => "Name or Password is Wrong!",
                    _ => "Сonnection error",
                };
                ModelState.AddModelError("LoginStatus", mes);
                return View(account);
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Login)
            };

            ClaimsIdentity claimsIdentity = new(claims, "Cookies");

            // Вход используя куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return Redirect(ReturnUrl ?? "/");
        }

        [HttpGet]
        public async Task<IActionResult> LogoutAsync(string ReturnUrl)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect(ReturnUrl ?? "/");
        }
    }
}
