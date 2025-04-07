using AppointmentSystem.Data;
using AppointmentSystem.Handlers.Doctor.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Serilog.Core;


namespace AppointmentSystem.Handlers.Doctor.Handlers
{
    class AddDoctorHandler:IRequestHandler<AddDoctorRequest,bool>
    {
        private readonly AppointmentDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AddDoctorHandler> _logger;
        public AddDoctorHandler(AppointmentDbContext context, UserManager<ApplicationUser> userManager, ILogger<AddDoctorHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> Handle(AddDoctorRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting to add new doctor with email: {Email}", request.Email);
            var user = new ApplicationUser { UserName = request.Email, Email = request.Email, FullName = request.FullName, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create user for doctor. Errors: {Errors}", errors);
             
                throw new Exception("Failed to create user");
            }
            _logger.LogInformation("User created successfully for doctor. Assigning role 'Doctor' to user ID: {UserId}", user.Id);
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
            _logger.LogInformation("Doctor details saved with DoctorId: {DoctorId}", doctor.DoctorId);
            var doctorSpecializations = request.SpecializationIds.Select(id => new DoctorSpecialization { DoctorId = doctor.DoctorId, SpecializationId = id }).ToList();
            _context.DoctorSpecializations.AddRange(doctorSpecializations);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Doctor specializations saved for DoctorId: {DoctorId}", doctor.DoctorId);

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
            _logger.LogInformation("Generated and saved slots for DoctorId: {DoctorId}", doctor.DoctorId);

            _logger.LogInformation("Successfully added new doctor with user ID: {UserId}", user.Id);
            return true;
        }

    }
}
