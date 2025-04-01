using AppointmentSystem.Data;
using AppointmentSystem.Handlers.DoctorSlot.Command;
using AppointmentSystem.Models;
using MediatR;


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
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
