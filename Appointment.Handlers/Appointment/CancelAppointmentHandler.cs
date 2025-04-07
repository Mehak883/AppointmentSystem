using AppointmentSystem.Data;
using AppointmentSystem.Handlers.Appointment.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace AppointmentSystem.Handlers.Appointment
{
    class CancelAppointmentHandler:IRequestHandler<CancelAppointmentRequest,bool>
    {
        private readonly AppointmentDbContext _context;
        private readonly ILogger<CancelAppointmentHandler> _logger;
        public CancelAppointmentHandler(AppointmentDbContext context,ILogger<CancelAppointmentHandler> logger)
        {
            _context = context;
            _logger = logger;

        }

        public async Task<bool> Handle(CancelAppointmentRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to cancel appointment with ID: {AppointmentId}", request.AppointmentId);
            var appointment = await _context.Appointments
                .Include(a => a.Slot)
                .FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                _logger.LogWarning("Appointment not found with ID: {AppointmentId}", request.AppointmentId);
                return false;
            }

            if (DateTime.Now >= appointment.Slot.Date.Add(appointment.Slot.StartTime))
            {
                _logger.LogWarning("Cannot cancel appointment with ID: {AppointmentId} as it is in the past", request.AppointmentId);
                return false;
            }

            appointment.Status = SlotStatus.Cancelled;
            appointment.Slot.Status = SlotStatus.Available;

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Appointment with ID: {AppointmentId} cancelled successfully", request.AppointmentId);
            return true;
        }
    }
}
