using AppointmentSystem.Data;
using AppointmentSystem.Handlers.Patient.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace AppointmentSystem.Handlers.Patient
{
    class RegisterPatientHandler:IRequestHandler<RegisterPatientRequest,bool>
    {
        private readonly AppointmentDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RegisterPatientHandler(AppointmentDbContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(RegisterPatientRequest request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new Exception("Failed to create user");

            if (!await _roleManager.RoleExistsAsync("Patient"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Patient"));
            }

            await _userManager.AddToRoleAsync(user, "Patient");

            var patient = new Models.Patient
            {
                UserId = user.Id,
                Gender = request.Gender
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync(cancellationToken);

            await _signInManager.SignInAsync(user, isPersistent: false);
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("UserId", user.Id);
            session.SetString("Role", "Patient");
            session.SetString("FullName", user.FullName);
            session.SetString("UserName", user.UserName);
            session.SetString("UserEmail", user.Email);
            return true;
        }
    }
}
