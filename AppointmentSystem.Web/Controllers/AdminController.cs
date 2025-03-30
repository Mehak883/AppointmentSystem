using AppointmentSystem.Dtos.Admin;
using AppointmentSystem.Handlers.Admin.Command;
using AppointmentSystem.Handlers.Admin.Query;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AppointmentSystem.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator, SignInManager<ApplicationUser> signInManager)
        {
            _mediator = mediator;
        }
        public IActionResult Dashboard()
        {

            //var session = _httpContextAccessor.HttpContext.Session;
            
            var session = HttpContext.Session;
           string userId= session.GetString("UserId");
           string role= session.GetString("Role");
            string fullname = session.GetString("FullName");

            string email = session.GetString("UserEmail");

            return View(new AdminResponseDto { UserId=userId,Role =role,Email=email,Fullname=fullname});
        }
        public IActionResult CreateAdmin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(AdminRequestDto adminRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return View(adminRequestDto);
            }

            var command = new CreateAdminRequest { Email = adminRequestDto.Email ,Fullname=adminRequestDto.FullName,Password=adminRequestDto.Password};
           bool isCreated =await  _mediator.Send(command);
            if (isCreated)
            {
                TempData["success"] = "Admin created successfully";

                return RedirectToAction("GetAllAdmin");
            }
            
                return View();
            
            
        }
        //public async Task<IActionResult> UpdateAdmin(AdminResponseDto adminResponseDto)
        //{
        //    AdminUpdateDto adminUpdateDto = new() { userId = adminResponseDto.UserId, Email = adminResponseDto.Email, FullName = adminResponseDto.Fullname };
        //    return View(adminUpdateDto);
        //}
        //[HttpPost]
        //public async Task<IActionResult> UpdateAdmin(AdminUpdateDto adminUpdateDto)
        //{
        //    var command = new UpdateAdminRequest { UserId = adminUpdateDto.userId, Email = adminUpdateDto.Email, FullName = adminUpdateDto.FullName };
        //      bool isUpdated=await _mediator.Send(command);
        //    if (isUpdated)
        //    {
        //        return RedirectToAction("GetAllAdmin");
        //    }
        //    return View();
        //}

        public async Task<IActionResult> UpdateAdmin(string id)
        {
            // Fetch admin details based on ID (assuming you have a method to do so)
            var admin = await GetAdminByIdAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            var adminUpdateDto = new AdminUpdateDto
            {
                UserId = admin.UserId,
                FullName = admin.Fullname,
                Email = admin.Email
            };

            return View(adminUpdateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdmin(AdminUpdateDto adminUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(adminUpdateDto);
            }

            var command = new UpdateAdminRequest
            {
                UserId = adminUpdateDto.UserId,
                Email = adminUpdateDto.Email,
                FullName = adminUpdateDto.FullName
            };

            bool isUpdated = await _mediator.Send(command);
            if (isUpdated)
            {
                TempData["success"] = "Admin updated successfully";

                return RedirectToAction("GetAllAdmin");
            }

            ModelState.AddModelError("", "Failed to update admin.");

            return View(adminUpdateDto);
        }
  
        [Route("Admin/ManageAdmins")]
        public async Task<IActionResult> GetAllAdmin()
        {
            var query = new GetAdminRequest { };
            ;
           List<AdminResponseDto> allAdmins=await  _mediator.Send(query);   
            return View(allAdmins);
        }

        private async Task<AdminResponseDto> GetAdminByIdAsync(string id)
        {
            var command = new GetByIdAdminRequest { UserId=id };
           AdminResponseDto adminResponseDto =await _mediator.Send(command);
            return adminResponseDto;
        }

        public async Task<IActionResult> DeleteAdmin(string id)
        {
            bool isDeleted = await _mediator.Send(new DeleteAdminRequest { UserId = id });
            if (isDeleted)
            {
                TempData["success"] = "Admin deleted successfully";

                return RedirectToAction("GetAllAdmin");
            }
            TempData["error"] = "Admin not Found";

            return NotFound();
        }
    }
}
