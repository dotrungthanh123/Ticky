using Ticky.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Ticky.Data;

namespace Ticky.Server.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly TickyContext _TickyContext;

        public AccountsController(IAccountRepository accountRepository, TickyContext TickyContext)
        {
            this.accountRepository = accountRepository;
            _TickyContext = TickyContext;
        }

        public IActionResult AccessDenied() => View();

        public IActionResult Login(string returnUrl = "/")
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var account = accountRepository.GetAccountWithUsernameAndPassword(model.Username, model.Password);
            if (account == null)
                return RedirectToAction("Login", "Accounts");

            string name = "";
            if (account.Role == 1)
            {
                name = _TickyContext.Admins.First(a => a.AccountId == account.AccountId).Name;
            }
            else if (account.Role == 2)
            {
                name = _TickyContext.Customers.First(c => c.AccountId == account.AccountId).Name;
            }
            else if (account.Role == 3)
            {
                name = _TickyContext.Retailers.First(r => r.AccountId == account.AccountId).Name;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
            };
            var identity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToAction("Index", "Home");
        }        

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}