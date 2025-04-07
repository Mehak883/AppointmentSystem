using AppointmentSystem.Handlers.Admin.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Serilog.Core;

public class UpdateAdminHandler : IRequestHandler<UpdateAdminRequest, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UpdateAdminHandler> _logger;

    public UpdateAdminHandler(UserManager<ApplicationUser> userManager, ILogger<UpdateAdminHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateAdminRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to update admin with ID: {UserId}", request.UserId);

        var admin = await _userManager.FindByIdAsync(request.UserId);
        if (admin == null) {
            _logger.LogWarning("Admin not found with ID: {UserId}", request.UserId);
            return false;
        }
    


        admin.FullName = request.FullName;
        admin.Email = request.Email;
        admin.EmailConfirmed = true;

        var result = await _userManager.UpdateAsync(admin);
        if (!result.Succeeded)
        {
            _logger.LogError("Failed to update admin with ID: {UserId}. Errors: {Errors}",
                request.UserId,
                string.Join(", ", result.Errors.Select(e => e.Description)));
            return false;
        }

        _logger.LogInformation("Admin updated successfully: {UserId}", request.UserId);
        return result.Succeeded;
    }
}
