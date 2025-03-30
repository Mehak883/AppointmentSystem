using AppointmentSystem.Dtos.Admin;
using AppointmentSystem.Handlers.Login.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
namespace AppointmentSystem.Handlers.Login
{
  public class LoginHandler : IRequestHandler<LoginRequest, AdminResponseDto>
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<AdminResponseDto> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if (!result.Succeeded)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);
            string role = roles.FirstOrDefault() ?? "Patient";

            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("UserId", user.Id);
            session.SetString("Role", role);
            session.SetString("FullName", user.FullName);
            session.SetString("UserName", user.UserName);
            session.SetString("UserEmail", user.Email);

        return new AdminResponseDto { UserId = user.Id, Email = user.Email, Fullname = user.FullName,Role= role };
            
        }
    }
    
}
