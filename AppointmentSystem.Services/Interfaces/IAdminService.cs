using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentSystem.Handlers.Admin.Command;
using AppointmentSystem.Dtos.Admin;
namespace AppointmentSystem.Services.Interfaces
{
    interface IAdminService
    {
        Task<List<AdminResponseDto>> GetAllAdminsAsync();
        Task<AdminResponseDto> GetAdminByIdAsync(string userId);
        Task<bool> CreateAdminAsync(CreateAdminRequest createAdminRequest);
        Task<bool> UpdateAdminAsync(UpdateAdminRequest updateAdminRequest);
        Task<bool> DeleteAdminAsync(string userId);

    }
}
