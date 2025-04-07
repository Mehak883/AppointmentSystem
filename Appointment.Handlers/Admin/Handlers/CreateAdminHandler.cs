using AppointmentSystem.Handlers.Admin.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;


namespace AppointmentSystem.Handlers.Admin.Handlers
{
    public class CreateAdminHandler : IRequestHandler<CreateAdminRequest, bool>
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CreateAdminHandler> _logger;

        public CreateAdminHandler(UserManager<ApplicationUser> userManager, ILogger<CreateAdminHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
     

        public async Task<bool> Handle(CreateAdminRequest request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Starting to create a new admin with email: {Email}", request.Email);
            var admin = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.Fullname,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(admin,request.Password);
            if (!result.Succeeded) {
                _logger.LogError("Failed to create admin user. Errors: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return false; }


            await _userManager.AddToRoleAsync(admin, "Admin");
            _logger.LogInformation("Admin user created and assigned to 'Admin' role: {Email}", request.Email);
            return true;
        }
    }
}
