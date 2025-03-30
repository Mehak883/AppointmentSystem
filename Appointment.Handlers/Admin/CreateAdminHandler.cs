using AppointmentSystem.Dtos.Admin;
using AppointmentSystem.Handlers.Admin.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Admin
{
    public class CreateAdminHandler : IRequestHandler<CreateAdminRequest, bool>
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public CreateAdminHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
     

        public async Task<bool> Handle(CreateAdminRequest request, CancellationToken cancellationToken)
        {
            var admin = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.Fullname,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(admin,request.Password);
            if (!result.Succeeded) return false;

            await _userManager.AddToRoleAsync(admin, "Admin");
            return true;
        }
    }
}
