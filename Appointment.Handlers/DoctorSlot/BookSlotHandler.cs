using AppointmentSystem.Data;
using AppointmentSystem.Handlers.DoctorSlot.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Handlers.DoctorSlot
{
    public class BookSlotHandler : IRequestHandler<BookSlotRequest, bool>
    {
        private readonly AppointmentDbContext _context;

        public BookSlotHandler(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(BookSlotRequest request, CancellationToken cancellationToken)
        {
            var slot = await _context.Slots
                .FirstOrDefaultAsync(s => s.SlotId == request.SlotId && s.Status == SlotStatus.Available, cancellationToken);

            if (slot == null)
            {
                return false;  // Slot not found or already booked
            }

            DateTime currentDateTime = DateTime.Now;
            DateTime slotDateTime = slot.Date.Date + slot.StartTime; // Combine date and start time

            // Ensure slot start time is in the future
            if (slotDateTime <= currentDateTime)
            {
                return false;
            }

            // Check if the patient already has an appointment at the same time
            var patientAppointments = await _context.Appointments
      .Where(a => a.PatientId == request.PatientId && a.AppointDate.Date == slot.Date.Date)
      .Include(a => a.Slot) // Include Slot details
      .ToListAsync(cancellationToken);

            // Check if there's a time conflict
            bool hasConflict = patientAppointments.Any(a => a.Slot != null &&
                a.Slot.StartTime == slot.StartTime);

            if (hasConflict)
            {
                return false; // Patient has another appointment at the same time
            }

            // Proceed with booking
            var appointment = new Models.Appointment
            {
                SlotId = request.SlotId,
                PatientId = request.PatientId,
                AppointDate = slot.Date,
                SpecializationId = request.SpecializationId,
                Status = SlotStatus.Booked
            };

            slot.Status = SlotStatus.Booked;

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
