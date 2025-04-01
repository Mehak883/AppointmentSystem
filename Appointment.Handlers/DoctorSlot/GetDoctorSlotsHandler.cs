using AppointmentSystem.Data;
using AppointmentSystem.Dtos.Slot;
using AppointmentSystem.Handlers.DoctorSlot.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace AppointmentSystem.Handlers.DoctorSlot
{

    public class GetDoctorSlotsHandler : IRequestHandler<GetDoctorSlotsQuery, List<SlotResponseDto>>
    {
        private readonly AppointmentDbContext _context;

        public GetDoctorSlotsHandler(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<List<SlotResponseDto>> Handle(GetDoctorSlotsQuery request, CancellationToken cancellationToken)
        {
            var slots = await _context.Slots
                .Where(s => s.DoctorId == request.DoctorId)
                .OrderBy(s => s.Date)
                .ThenBy(s => s.StartTime)
                .Select(s => new SlotResponseDto
                {
                    SlotId = s.SlotId,
                    Date = s.Date,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Status = s.Status.ToString() 
                })
                .ToListAsync(cancellationToken);
            return slots;
        }
    }
}