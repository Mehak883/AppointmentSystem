using AppointmentSystem.Data;
using AppointmentSystem.Handlers.DoctorSlot.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace AppointmentSystem.Handlers.DoctorSlot
{
   
    public class MarkSlotAsCompletedHandler : IRequestHandler<MarkSlotAsCompletedRequest, bool>
    {
        private readonly AppointmentDbContext _context;

        public MarkSlotAsCompletedHandler(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(MarkSlotAsCompletedRequest request, CancellationToken cancellationToken)
        {
            var slot = await _context.Slots.FindAsync(request.SlotId);
            if (slot == null) return false;

            slot.Status = SlotStatus.Completed;
            _context.Slots.Update(slot);

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.SlotId == request.SlotId, cancellationToken);

            if (appointment != null)
            {
                appointment.Status = SlotStatus.Completed;
                _context.Appointments.Update(appointment);
            }

            // Save changes
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
