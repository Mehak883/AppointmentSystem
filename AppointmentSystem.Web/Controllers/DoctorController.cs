using AppointmentSystem.Dtos.Doctor;
using AppointmentSystem.Handlers.Doctor.Query;
using AppointmentSystem.Handlers.DoctorSlot.Command;
using AppointmentSystem.Handlers.DoctorSlot.Query;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Web.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly IMediator _mediator;
       

        public DoctorController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Dashboard()
        {
            var session = HttpContext.Session;
            string userId = session.GetString("UserId")!;
            string role = session.GetString("Role")!;
            string fullname = session.GetString("FullName")!;
            string email = session.GetString("UserEmail")!;
            var doctor = await _mediator.Send(new GetDoctorByUserIdQuery(userId)); 
            var slots = await _mediator.Send(new GetDoctorSlotsQuery(doctor.DoctorId, DateTime.Today));
            var groupedSlots = slots
                .GroupBy(s => s.Date.Date)  
                .OrderBy(g => g.Key)  
                .ToList();
            var viewModel = new DoctorDashboardDto
            {
                DoctorId = doctor.DoctorId,
                Specializations = doctor.Specializations,
                UserId = userId, 
                Fullname = fullname,
                Email = email,
                Role = role,
                SlotsGroupedByDate = groupedSlots,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsCompleted(int slotId)
        {
            var result = await _mediator.Send(new MarkSlotAsCompletedRequest(slotId));

            if (!result) return NotFound();

            return RedirectToAction("Dashboard");
        }
    }

}


