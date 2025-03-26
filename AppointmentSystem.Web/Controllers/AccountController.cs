using AppointmentSystem.Web.DTOs;
using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Handlers.Login.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
namespace AppointmentSystem.Web.Controllers
{

    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IMediator mediator, SignInManager<ApplicationUser> signInManager)
        {
            _mediator = mediator;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var command = new LoginRequest { Email = loginDto.Email, Password = loginDto.Password };
            var isAuthenticated = await _mediator.Send(command);

            if (isAuthenticated)
            {
                var userRole = HttpContext.Session.GetString("Role");

                if (userRole == "Admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (userRole == "Doctor")
                {
                    return RedirectToAction("Dashboard", "Doctor");
                }
                else if (userRole == "Patient")
                {
                    return RedirectToAction("Profile", "Patient");
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Account");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(loginDto);
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login","Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
