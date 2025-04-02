using Microsoft.AspNetCore.Mvc;
using AppointmentSystem.Handlers.Login.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AppointmentSystem.Dtos.Admin;
using AppointmentSystem.Dtos.Identity;
using AppointmentSystem.Handlers.Patient.Command;
using AppointmentSystem.Dtos.Patient;

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
            var userRole = HttpContext.Session.GetString("Role");

            if (!string.IsNullOrEmpty(userRole))
            {
                // Redirect users to their respective dashboards
                return RedirectToAction("RedirectToDashboard");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var command = new LoginRequest { Email = loginDto.Email, Password = loginDto.Password };
            var user = await _mediator.Send(command);

            if (user != null)
            {
                return RedirectToAction("RedirectToDashboard");
            }

            ModelState.AddModelError("InvalidCredentials", "Invalid email or password.");
            return View(loginDto);
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(RegisterPatientRequestDto request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var command = new RegisterPatientRequest { Email = request.Email, FullName = request.FullName, Gender = request.Gender, Password = request.Password };
            var result = await _mediator.Send(command);
            if (result)
                return RedirectToAction("Dashboard", "Patient"); // Redirect to patient dashboard

            ModelState.AddModelError("", "Signup failed. Please try again.");
            return View(request);
        }
        public IActionResult RedirectToDashboard()
        {
            var userRole = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userRole))
            {
                return RedirectToAction("Login");
            }

            var currentPath = HttpContext.Request.Path.Value.ToLower();

            if (userRole == "Admin" || userRole == "SuperAdmin")
            {
                if (!currentPath.StartsWith("/admin/dashboard"))
                    return RedirectToAction("Dashboard", "Admin");
            }
            else if (userRole == "Doctor")
            {
                if (!currentPath.StartsWith("/doctor/dashboard"))
                    return RedirectToAction("Dashboard", "Doctor");
            }
            else if (userRole == "Patient")
            {
                if (!currentPath.StartsWith("/patient/dashboard"))
                    return RedirectToAction("Dashboard", "Patient");
            }
            else
            {
                return RedirectToAction("AccessDenied");
            }

            return RedirectToAction("Login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Identity.Application"); // Ensure session cookies are cleared
            return RedirectToAction("Login");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ChangePasswordRequestDto changePasswordRequestDto)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login");
            }

            var command = new ChangePasswordRequest
            {
                UserId = userId,
                OldPassword = changePasswordRequestDto.OldPassword,
                NewPassword = changePasswordRequestDto.NewPassword
            };

            var success = await _mediator.Send(command);

            if (success)
            {
                HttpContext.Session.Clear(); // Log out the user
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("Error", "Password reset failed.");
            return View("ResetPassword"); // Adjust based on your UI
        }


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
