using AppointmentSystem.Handlers.Admin.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
namespace AppointmentSystem.Handlers.Admin.Handlers
{
 public class DeleteAdminHandler : IRequestHandler<DeleteAdminRequest, bool>

    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DeleteAdminHandler> _logger;

        public DeleteAdminHandler(UserManager<ApplicationUser> userManager, ILogger<DeleteAdminHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<bool> Handle(DeleteAdminRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete admin with ID: {UserId}", request.UserId);

            var admin = await _userManager.FindByIdAsync(request.UserId);
            if (admin == null)
            {
                _logger.LogWarning("Admin with ID: {UserId} not found", request.UserId);
                return false;
            }

            var result = await _userManager.DeleteAsync(admin);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to delete admin with ID: {UserId}. Errors: {Errors}",
                    request.UserId,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }

            return result.Succeeded;

        }
    }
}
