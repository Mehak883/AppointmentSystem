using AppointmentSystem.Dtos.Admin;
using AppointmentSystem.Handlers.Admin.Query;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace AppointmentSystem.Handlers.Admin.Handlers
{
    class GetAllAdminHandler : IRequestHandler<GetAdminRequest, List<AdminResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<GetAllAdminHandler> _logger;
        public GetAllAdminHandler(UserManager<ApplicationUser> userManager,ILogger<GetAllAdminHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<List<AdminResponseDto>> Handle(GetAdminRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all users with 'Admin' role");
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            _logger.LogInformation("Found {Count} admin(s)", admins.Count);
            return admins.Select(a => new AdminResponseDto
            {
                UserId = a.Id,
                Fullname = a.FullName,
                Email = a.Email,
                Role="Admin"

            }).ToList();
        }
    }
}
