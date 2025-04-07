using AppointmentSystem.Dtos.Admin;
using AppointmentSystem.Handlers.Admin.Query;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace AppointmentSystem.Handlers.Admin.Handlers
{
    class GetByIdAdminHandler : IRequestHandler<GetByIdAdminRequest, AdminResponseDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<GetByIdAdminHandler> _logger;
        public GetByIdAdminHandler(UserManager<ApplicationUser> userManager,ILogger<GetByIdAdminHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
               
        }
        public async Task<AdminResponseDto> Handle(GetByIdAdminRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching admin user with ID: {UserId}", request.UserId);
            var admin = await _userManager.FindByIdAsync(request.UserId);
            if (admin == null)
            {
                _logger.LogWarning("Admin user not found with ID: {UserId}", request.UserId);
                return null;
            }

            _logger.LogInformation("Admin user found: {Email}", admin.Email);
            return new AdminResponseDto
            {
                UserId = admin.Id,
                Fullname = admin.FullName,
                Role="Admin",
                Email = admin.Email,
            };
        }
    }
}
