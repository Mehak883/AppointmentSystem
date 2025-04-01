using AppointmentSystem.Dtos.Admin;
using AppointmentSystem.Dtos.Specialization;
using AppointmentSystem.Handlers.Admin.Command;
using AppointmentSystem.Handlers.Admin.Query;
using AppointmentSystem.Handlers.DoctorSlot.Command;
using AppointmentSystem.Handlers.Specialization.Command;
using AppointmentSystem.Handlers.Specializations.Command;
using AppointmentSystem.Handlers.Specializations.Query;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
        public async Task<IActionResult> UpdateAdmin(string id)
        {
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
        public async Task<IActionResult> DeleteSpecialization(int id)
        {
            bool isDeleted = await _mediator.Send(new DeleteSpecializationRequest {SpecializationId= id  });
            if (isDeleted)
            {
                TempData["success"] = "Specialization deleted successfully";

                return RedirectToAction("ManageSpecialization");
            }
            TempData["error"] = "Specialization not Found";

            return NotFound();
        }


        public async Task<IActionResult> ManageSpecialization()
        {
            var query = new GetAllSpecializationQuery { };
            List<SpecializationResponseDto> specializationResponseDtos = await _mediator.Send(query);
            return View(specializationResponseDtos);
        }

        public IActionResult AddSpecialization()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSpecialization(SpecializationRequestDto specializationRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return View(specializationRequestDto);
            }

            var command = new CreateSpecializationRequest { SpecializationName= specializationRequestDto.SpecializationName };
            bool isCreated = await _mediator.Send(command);
            if (isCreated)
            {
                TempData["success"] = "Specialization created successfully";

                return RedirectToAction("ManageSpecialization");
            }

            return View();
        }
        public async Task<IActionResult> UpdateSpecialization(int id)
        {
            var specialization = await GetSpecializationById(id);
            if (specialization== null)
            {
                return NotFound();
            }

            var specializationUpdateDto = new SpecializationResponseDto
            {
                SpecializationId = specialization.SpecializationId,
                SpecializationName = specialization.SpecializationName
            };

            return View(specializationUpdateDto);
        }
        public async Task<SpecializationResponseDto> GetSpecializationById(int id)
        {
            var query = new GetByIdSpecializationQuery{SpecializationId = id };
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSpecialization(SpecializationResponseDto specializationResponseDto)
        {
            if (!ModelState.IsValid)
            {
                return View(specializationResponseDto);
            }

            var command = new UpdateSpecializationRequest
            {
                SpecializationId = specializationResponseDto.SpecializationId,
                SpecializationName = specializationResponseDto.SpecializationName
            };

            bool isUpdated = await _mediator.Send(command);
            if (isUpdated)
            {
                TempData["success"] = "Specialization updated successfully";

                return RedirectToAction("ManageSpecialization");
                
            }

            ModelState.AddModelError("", "Failed to update Specialization.");

            return View(specializationResponseDto);
        }
        [HttpPost]
        public async Task<IActionResult> GenerateTomorrowSlots()
        {
            var result = await _mediator.Send(new GenerateTomorrowSlotsRequest());

            if (!result) TempData["Error"] = "Slots for tomorrow already exist!";
            else TempData["Success"] = "Slots for tomorrow have been generated successfully.";

            return RedirectToAction("Dashboard");
        }
    }
}

