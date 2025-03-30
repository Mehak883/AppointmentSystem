using AppointmentSystem.Dtos.Admin;
using AppointmentSystem.Handlers.Admin.Command;
using AppointmentSystem.Models;
using AppointmentSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Services
{

    class AdminService : IAdminService
    {

         private readonly UserManager<ApplicationUser> _userManager;

    public AdminService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
        public async Task<List<AdminResponseDto>> GetAllAdminsAsync()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            return admins.Select(a => new AdminResponseDto
            {
               UserId = a.Id,
               Fullname = a.FullName,
                Email = a.Email,
              
            }).ToList();
        }

        public async Task<AdminResponseDto> GetAdminByIdAsync(string userId)
        {
            var admin = await _userManager.FindByIdAsync(userId);
            if (admin == null) return null;

            return new AdminResponseDto
            {
                UserId = admin.Id,
                Fullname = admin.FullName,
                Email = admin.Email,
            };
        }

        public async Task<bool> CreateAdminAsync(CreateAdminRequest addAdminRequest)
        {
            var admin = new ApplicationUser
            {
                UserName = addAdminRequest.Email,
                Email = addAdminRequest.Email,
                FullName = addAdminRequest.Fullname,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(admin, addAdminRequest.Password);
            if (!result.Succeeded) return false;

            // Assign the admin role
            await _userManager.AddToRoleAsync(admin, "Admin");
            return true;
        }

        public async Task<bool> UpdateAdminAsync(UpdateAdminRequest updateAdminRequest)
        {
            var admin = await _userManager.FindByIdAsync(updateAdminRequest.UserId);
            if (admin == null) return false;

            admin.FullName = updateAdminRequest.FullName;
            admin.Email = updateAdminRequest.Email;
            admin.EmailConfirmed = true;
            var result = await _userManager.UpdateAsync(admin);
            return result.Succeeded;
        }

        public async Task<bool> DeleteAdminAsync(string userId)
        {
            var admin = await _userManager.FindByIdAsync(userId);
            if (admin == null) return false;

            var result = await _userManager.DeleteAsync(admin);
            return result.Succeeded;
        }
    }
}
