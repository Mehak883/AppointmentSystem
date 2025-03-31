using AppointmentSystem.Data;
using AppointmentSystem.Handlers.Doctor.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace AppointmentSystem.Handlers.Doctor.Handlers
{
    class AddDoctorHandler:IRequestHandler<AddDoctorRequest,bool>
    {
        private readonly AppointmentDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddDoctorHandler(AppointmentDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> Handle(AddDoctorRequest request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser { UserName = request.Email, Email = request.Email, FullName = request.FullName, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, request.Password); 

            if (!result.Succeeded)
                throw new Exception("Failed to create user");
            await _userManager.AddToRoleAsync(user, "Doctor");
            var doctor = new Models.Doctor
            {
                UserId = user.Id,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                SlotDuration = request.SlotDuration,
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync(cancellationToken);

            var doctorSpecializations = request.SpecializationIds.Select(id => new DoctorSpecialization { DoctorId = doctor.DoctorId, SpecializationId = id }).ToList();
            _context.DoctorSpecializations.AddRange(doctorSpecializations);
            await _context.SaveChangesAsync(cancellationToken);

            var today = DateTime.Today;
            var slots = new List<Slot>();
            for (int i = 0; i < 2; i++)
            {
                var date = today.AddDays(i);
                for (var time = request.StartTime; time < request.EndTime; time = time.Add(TimeSpan.FromMinutes(request.SlotDuration)))
                {
                    slots.Add(new Slot { DoctorId = doctor.DoctorId, Date = date, StartTime = time, EndTime = time.Add(TimeSpan.FromMinutes(request.SlotDuration)) });
                }
            }

            _context.Slots.AddRange(slots);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

    }
}
