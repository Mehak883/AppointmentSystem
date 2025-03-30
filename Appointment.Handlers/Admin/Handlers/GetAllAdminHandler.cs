using AppointmentSystem.Dtos.Admin;
using AppointmentSystem.Handlers.Admin.Query;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Admin.Handlers
{
    class GetAllAdminHandler : IRequestHandler<GetAdminRequest, List<AdminResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllAdminHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<List<AdminResponseDto>> Handle(GetAdminRequest request, CancellationToken cancellationToken)
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
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
