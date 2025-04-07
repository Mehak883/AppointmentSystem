using AppointmentSystem.Data;
using AppointmentSystem.Handlers.Doctor.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentSystem.Handlers.Doctor.Handlers
{
    class UpdateDoctorHandler:IRequestHandler<UpdateDoctorRequest,bool>
    {
        private readonly AppointmentDbContext _context;
        private readonly ILogger<UpdateDoctorHandler> _logger;
        public UpdateDoctorHandler(AppointmentDbContext context, ILogger<UpdateDoctorHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateDoctorRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting update process for DoctorId: {DoctorId}", request.DoctorId);
            var doctor = await _context.Doctors.Include(d => d.DoctorSpecializations).FirstOrDefaultAsync(d => d.DoctorId == request.DoctorId);
            if (doctor == null) {
                _logger.LogWarning("Doctor not found with DoctorId: {DoctorId}", request.DoctorId);
                throw new Exception("Doctor not found"); }

            doctor.StartTime = request.StartTime;
            doctor.EndTime = request.EndTime;
            doctor.SlotDuration = request.SlotDuration;

            _logger.LogInformation("Updating doctor timings and slot duration.");

            _context.DoctorSpecializations.RemoveRange(doctor.DoctorSpecializations);

            _logger.LogInformation("Removed existing specializations for DoctorId: {DoctorId}", doctor.DoctorId);
            var newSpecializations = request.SpecializationIds.Select(id => new DoctorSpecialization { DoctorId = doctor.DoctorId, SpecializationId = id }).ToList();
            _context.DoctorSpecializations.AddRange(newSpecializations);
            _logger.LogInformation("Added {Count} new specializations for DoctorId: {DoctorId}", newSpecializations.Count, doctor.DoctorId);

       

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Doctor with DoctorId: {DoctorId} updated successfully.", doctor.DoctorId);
            return true;
        }
    }
}
