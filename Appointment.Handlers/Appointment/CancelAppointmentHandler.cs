using AppointmentSystem.Data;
using AppointmentSystem.Handlers.Appointment.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace AppointmentSystem.Handlers.Appointment
{
    class CancelAppointmentHandler:IRequestHandler<CancelAppointmentRequest,bool>
    {
        private readonly AppointmentDbContext _context;

        public CancelAppointmentHandler(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CancelAppointmentRequest request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Slot)
                .FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId, cancellationToken);

            if (appointment == null || DateTime.Now >= appointment.Slot.Date.Add(appointment.Slot.StartTime))
            {
                return false; // Cannot cancel past appointments
            }

            appointment.Status = SlotStatus.Cancelled;
            appointment.Slot.Status = SlotStatus.Available;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
