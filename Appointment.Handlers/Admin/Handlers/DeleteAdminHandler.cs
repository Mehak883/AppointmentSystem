using AppointmentSystem.Handlers.Admin.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
namespace AppointmentSystem.Handlers.Admin.Handlers
{
 public class DeleteAdminHandler : IRequestHandler<DeleteAdminRequest, bool>

    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteAdminHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> Handle(DeleteAdminRequest request, CancellationToken cancellationToken)
        {
            var admin = await _userManager.FindByIdAsync(request.UserId);
            if (admin == null) return false;

            var result = await _userManager.DeleteAsync(admin);
            return result.Succeeded;
        }
    }
}
