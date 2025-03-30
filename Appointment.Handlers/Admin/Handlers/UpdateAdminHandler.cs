using AppointmentSystem.Handlers.Admin.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

public class UpdateAdminHandler : IRequestHandler<UpdateAdminRequest, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateAdminHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(UpdateAdminRequest request, CancellationToken cancellationToken)
    {
        var admin = await _userManager.FindByIdAsync(request.UserId);
        if (admin == null) return false;

        admin.FullName = request.FullName;
        admin.Email = request.Email;
        admin.EmailConfirmed = true;

        var result = await _userManager.UpdateAsync(admin);
        return result.Succeeded;
    }
}
