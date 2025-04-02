using AppointmentSystem.Data;
using AppointmentSystem.Dtos.Appointment;
using AppointmentSystem.Handlers.Appointment.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace AppointmentSystem.Handlers.Appointment
{ 
        public class GetAppointmentsHandler : IRequestHandler<GetAppointmentsQuery, List<AppointmentResponseDto>>
        {
            private readonly AppointmentDbContext _context;

            public GetAppointmentsHandler(AppointmentDbContext context)
            {
                _context = context;
            }

            public async Task<List<AppointmentResponseDto>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
            {
                return await _context.Appointments
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
            }
        }
    }


