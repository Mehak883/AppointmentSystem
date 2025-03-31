using AppointmentSystem.Dtos.Doctor;
using AppointmentSystem.Dtos.Specialization;
using AppointmentSystem.Handlers.Doctor.Command;
using AppointmentSystem.Handlers.Doctor.Query;
using AppointmentSystem.Handlers.Specializations.Command;
using AppointmentSystem.Handlers.Specializations.Query;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace AppointmentSystem.Web.Controllers
{

    public class ManageDoctorController : Controller
    {

        private readonly IMediator _mediator;

        public ManageDoctorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> ViewAll(int? specializationId)
        {
            List<DoctorResponseDto> doctors;

            var specializations = await _mediator.Send(new GetAllSpecializationQuery());
            ViewBag.Specializations = specializations;

            if (specializationId.HasValue)
            {
                doctors = await _mediator.Send(new GetDoctorsBySpecializationIdQuery(specializationId.Value));
            }
            else
            {
               doctors= await _mediator.Send(new GetAllDoctorsQuery());
            }

                return View(doctors);
        }
        public async Task<List<SpecializationResponseDto>> GetAllSpecialization()
        {
            var query = new GetAllSpecializationQuery { };
             List<SpecializationResponseDto> specializations= await _mediator.Send(query);
            return specializations;
         }

        public async Task<IActionResult> AddDoctor()
        {

            ViewBag.Specializations = await GetAllSpecialization(); 
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoctor(DoctorRequestDto doctorRequestDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Specializations = await GetAllSpecialization();
                return View();
            }

            AddDoctorRequest command = new AddDoctorRequest { Email = doctorRequestDto.Email,FullName = doctorRequestDto.FullName,Password= doctorRequestDto.Password,EndTime = doctorRequestDto.EndTime,
            SlotDuration = doctorRequestDto.SlotDuration, SpecializationIds = doctorRequestDto.SpecializationIds.ToList(),
                StartTime=doctorRequestDto.StartTime};
            var isCreated= await _mediator.Send(command);
            if (isCreated)
            {
                return RedirectToAction("ViewAll");
            }
            return View();
        }

        public async Task<IActionResult> DeleteDoctor(int id)
        {
            bool isDeleted = await _mediator.Send(new DeleteDoctorRequest { DoctorId = id });
            if (isDeleted)
            {
                TempData["success"] = "Doctor deleted successfully";

                return RedirectToAction("ViewAll");
            }
            TempData["error"] = "Specialization not Found";

            return NotFound();
        }
        public async Task<DoctorResponseDto> GetDoctorById(int id)
        {
            return await _mediator.Send(new GetDoctorByIdQuery { DoctorId = id }); 
        }
        public async Task<IActionResult> UpdateDoctor(int id){
            var doctor = await GetDoctorById(id);

            if (doctor == null)
            {
                return NotFound();
            }
            ViewBag.Specializations = await GetAllSpecialization();
            var doctorUpdateDto = new DoctorUpdateDto
            {
                DoctorId = doctor.DoctorId,
                EndTime = doctor.EndTime,
                SlotDuration = doctor.SlotDuration,
                SpecializationIds = doctor.Specializations.Select(ds => ds.SpecializationId).ToList(),
                StartTime = doctor.StartTime
           
            };

            return View(doctorUpdateDto);

        }


        [HttpPost]
        public async Task<IActionResult> UpdateDoctor(DoctorUpdateDto doctorUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Specializations = await GetAllSpecialization();
                return View(doctorUpdateDto);
            }

            var command = new UpdateDoctorRequest
            {
                DoctorId = doctorUpdateDto.DoctorId,
                EndTime = doctorUpdateDto.EndTime,
                StartTime = doctorUpdateDto.StartTime,
                SlotDuration = doctorUpdateDto.SlotDuration,
                SpecializationIds = doctorUpdateDto.SpecializationIds
            };

            bool isUpdated = await _mediator.Send(command);
            if (isUpdated)
            {
                TempData["success"] = "Docotr updated successfully";

                return RedirectToAction("ViewAll");

            }
            ViewBag.Specializations = await GetAllSpecialization();
            ModelState.AddModelError("", "Failed to update Doctor.");

            return View(doctorUpdateDto);
        }

    }
}
