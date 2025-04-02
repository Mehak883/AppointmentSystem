using AppointmentSystem.Dtos.Appointment;
using AppointmentSystem.Dtos.Patient;
using AppointmentSystem.Dtos.Specialization;
using AppointmentSystem.Handlers.Appointment.Command;
using AppointmentSystem.Handlers.Appointment.Query;
using AppointmentSystem.Handlers.Doctor.Query;
using AppointmentSystem.Handlers.DoctorSlot.Command;
using AppointmentSystem.Handlers.DoctorSlot.Query;
using AppointmentSystem.Handlers.Patient.Query;
using AppointmentSystem.Handlers.Specializations.Query;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppointmentSystem.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly IMediator _mediator;
        private int SpecializationId { get; set; }
        public PatientController(IMediator mediatr)
        {
            _mediator = mediatr;
        }
        public async Task<IActionResult> Dashboard()
        {
            var session = HttpContext.Session;
            string userId = session.GetString("UserId")!;
            string role = session.GetString("Role")!;
            string fullname = session.GetString("FullName")!;
            string email = session.GetString("UserEmail")!;
            PatientResponseDto patient = await _mediator.Send(new GetPatientByUserIdQuery(userId));
            ViewBag.Specializations = await GetAllSpecialization();

            PatientDashboardDto patientDashboardDto = new PatientDashboardDto { Email = email, FullName = fullname, Gender = patient.Gender, PatientId = patient.PatientId, Role = role, UserId = patient.UserId };

            return View(patientDashboardDto);
        }

        public async Task<List<SpecializationResponseDto>> GetAllSpecialization()
        {
            var query = new GetAllSpecializationQuery { };
            List<SpecializationResponseDto> specializations = await _mediator.Send(query);
            return specializations;
        }

        public async Task<IActionResult> DoctorsBySpecializationAsync(int specializationId) {
            var doctors = await _mediator.Send(new GetDoctorsBySpecializationIdQuery(specializationId));
            ViewBag.SpecializationId = specializationId;

            return View(doctors);
        }
  
        public async Task<IActionResult> Slots(int doctorId, int specializationId)
        {
            var slots = await _mediator.Send(new GetAvailableDoctorSlotQuery(doctorId));

            var groupedSlots = slots
                .GroupBy(s => s.Date.Date)
                .ToDictionary(g => g.Key, g => g.ToList());
            ViewBag.SpecializationId = specializationId;
            return View(groupedSlots);
        }

       
        public async Task<IActionResult> Appointment()
        {
            var session = HttpContext.Session;
            string userId = session.GetString("UserId")!;
            PatientResponseDto patient = await _mediator.Send(new GetPatientByUserIdQuery(userId));
                List<AppointmentResponseDto> appointments = await _mediator.Send(new GetAppointmentsQuery(patient.PatientId));
                return View(appointments);
            
        }


        [HttpPost]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var success = await _mediator.Send(new CancelAppointmentRequest(appointmentId));
            if (success)
            {
                TempData["SuccessMessage"] = "Appointment canceled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to cancel appointment.";
            }
            return RedirectToAction("Appointment");
        }

        [HttpPost]
        public async Task<IActionResult> BookSlot(int slotId, int specializationId)
        {
            var session = HttpContext.Session;
            string userId = session.GetString("UserId")!;
            PatientResponseDto patient = await _mediator.Send(new GetPatientByUserIdQuery(userId));

            var result = await _mediator.Send(new BookSlotRequest(slotId, patient.PatientId, specializationId));  // ✅ Pass specializationId here

            if (result)
            {
                TempData["Success"] = "Slot booked successfully!";
                return RedirectToAction("Appointments");
            }
            else
            {
                TempData["Error"] = "Failed to book slot. It may have been already booked.";
                return RedirectToAction("Slots", new { doctorId = patient.PatientId, specializationId });
            }
        }

    }
}
