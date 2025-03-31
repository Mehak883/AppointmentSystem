using AppointmentSystem.Data;
using AppointmentSystem.Handlers.Doctor.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Handlers.Doctor.Handlers
{
    class UpdateDoctorHandler:IRequestHandler<UpdateDoctorRequest,bool>
    {
        private readonly AppointmentDbContext _context;

        public UpdateDoctorHandler(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateDoctorRequest request, CancellationToken cancellationToken)
        {
            var doctor = await _context.Doctors.Include(d => d.DoctorSpecializations).FirstOrDefaultAsync(d => d.DoctorId == request.DoctorId);
            if (doctor == null) throw new Exception("Doctor not found");

            doctor.StartTime = request.StartTime;
            doctor.EndTime = request.EndTime;
            doctor.SlotDuration = request.SlotDuration;

          
            _context.DoctorSpecializations.RemoveRange(doctor.DoctorSpecializations);
            var newSpecializations = request.SpecializationIds.Select(id => new DoctorSpecialization { DoctorId = doctor.DoctorId, SpecializationId = id }).ToList();
            _context.DoctorSpecializations.AddRange(newSpecializations);

          
            var today = DateTime.Today;
            var slots = await _context.Slots.Where(s => s.DoctorId == doctor.DoctorId && (s.Date == today || s.Date == today.AddDays(1))).ToListAsync();
            _context.Slots.RemoveRange(slots);

            var newSlots = new List<Slot>();
            for (int i = 0; i < 2; i++)
            {
                var date = today.AddDays(i);
                for (var time = request.StartTime; time < request.EndTime; time = time.Add(TimeSpan.FromMinutes(request.SlotDuration)))
                {
                    newSlots.Add(new Slot { DoctorId = doctor.DoctorId, Date = date, StartTime = time, EndTime = time.Add(TimeSpan.FromMinutes(request.SlotDuration)) });
                }
            }
            _context.Slots.AddRange(newSlots);

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
