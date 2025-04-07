using AppointmentSystem.Data;
using AppointmentSystem.Dtos.Appointment;
using AppointmentSystem.Handlers.Appointment.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace AppointmentSystem.Handlers.Appointment
{ 
        public class GetAppointmentsHandler : IRequestHandler<GetAppointmentsQuery, List<AppointmentResponseDto>>
        {
            private readonly AppointmentDbContext _context;
        private readonly ILogger<GetAppointmentsHandler> _logger;
        public GetAppointmentsHandler(AppointmentDbContext context, ILogger<GetAppointmentsHandler> logger)
            {
                _context = context;
            _logger = logger;
            }

        public async Task<List<AppointmentResponseDto>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching appointments for patient ID: {PatientId}", request.PatientId);
            try
            {
                var appointments = await _context.Appointments
                    .Include(a => a.Slot)
                        .ThenInclude(s => s.Doctor)
                    .Include(a => a.Specialization)
                    .Where(a => a.PatientId == request.PatientId)
                    .Select(a => new AppointmentResponseDto
                    {
                        AppointmentId = a.AppointmentId,
                        PatientId = a.PatientId,
                        SlotId = a.SlotId,
                        SlotDate = a.Slot.Date,
                        StartTime = a.Slot.StartTime,
                        EndTime = a.Slot.EndTime,
                        Status = a.Status,
                        AppointDate = a.AppointDate,
                        DoctorId = a.Slot.DoctorId,
                        DoctorName = a.Slot.Doctor.User.FullName,
                        SpecializationId = a.SpecializationId,
                        SpecializationName = a.Specialization.SpecializationName
                    })
                    .OrderByDescending(a => a.AppointDate)
                    .ToListAsync(cancellationToken);
                return appointments;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching appointments for patient ID: {PatientId}", request.PatientId);
                return new List<AppointmentResponseDto>();
            }
            }
        }
    }


