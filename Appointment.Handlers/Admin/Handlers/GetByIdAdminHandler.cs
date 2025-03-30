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
    class GetByIdAdminHandler : IRequestHandler<GetByIdAdminRequest, AdminResponseDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetByIdAdminHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<AdminResponseDto> Handle(GetByIdAdminRequest request, CancellationToken cancellationToken)
        {
            var admin = await _userManager.FindByIdAsync(request.UserId);
            if (admin == null) return null;

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
