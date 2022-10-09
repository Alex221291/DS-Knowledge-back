using System.Net;
using System.Security.Claims;
using DS_KnowledgeWebApi.Models;
using DS_KnowledgeWebApi.Services;
using DS_KnowledgeWebApi.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DS_KnowledgeWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [Route("Login")]
        [HttpPost]
        public async Task<ObjectResult> Login(LoginUserViewModel loginUser)
        {
            var user = await _accountService.Login(loginUser);
            if (user == null) return BadRequest(user);
            await Authenticate(user);
            return Ok(user);

        }

        [Route("Register")]
        [HttpPost]
        public async Task<ObjectResult> Register(LoginUserViewModel loginUser)
        {
            var user = await _accountService.Register(loginUser);
            if (user == null) return BadRequest(user);
            await Authenticate(user);
            return Ok(user);

        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email!),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role!.Name!)
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
