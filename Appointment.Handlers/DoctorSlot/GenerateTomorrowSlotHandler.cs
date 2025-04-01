using AppointmentSystem.Data;
using AppointmentSystem.Handlers.DoctorSlot.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.DoctorSlot
{
    class GenerateTomorrowSlotHandler:IRequestHandler<GenerateTomorrowSlotsRequest,bool>
    {
        private readonly AppointmentDbContext _context;

        public GenerateTomorrowSlotHandler(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(GenerateTomorrowSlotsRequest request, CancellationToken cancellationToken)
        {
            var tomorrow = DateTime.Today.AddDays(1);

            var doctors = await _context.Doctors.ToListAsync(cancellationToken);

            var newSlots = new List<Slot>();

            foreach (var doctor in doctors)
            {
                var existingSlots = await _context.Slots
                    .AnyAsync(s => s.DoctorId == doctor.DoctorId && s.Date == tomorrow, cancellationToken);

                if (!existingSlots)
                {
                 
                    for (var time = doctor.StartTime; time < doctor.EndTime; time = time.Add(TimeSpan.FromMinutes(doctor.SlotDuration)))
                    {
                        newSlots.Add(new Slot
                        {
                            DoctorId = doctor.DoctorId,
                            Date = tomorrow,
                            StartTime = time,
                            EndTime = time.Add(TimeSpan.FromMinutes(doctor.SlotDuration)),
                            Status = SlotStatus.Available
                        });
                    }
                }
            }

            if (newSlots.Count > 0)
            {
                _context.Slots.AddRange(newSlots);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return true;
        }
    }
}
