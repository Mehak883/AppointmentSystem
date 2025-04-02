using AppointmentSystem.Data;
using AppointmentSystem.Dtos.Slot;
using AppointmentSystem.Handlers.DoctorSlot.Query;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace AppointmentSystem.Handlers.DoctorSlot
{
    class GetAvailableDoctorSlotHandler: IRequestHandler<GetAvailableDoctorSlotQuery, List<SlotPatientResponseDto>>
        {
            private readonly AppointmentDbContext _context;

            public GetAvailableDoctorSlotHandler(AppointmentDbContext context)
            {
                _context = context;
            }

            public async Task<List<SlotPatientResponseDto>> Handle(GetAvailableDoctorSlotQuery request, CancellationToken cancellationToken)
            {
            
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            List<Slot> slots = await _context.Slots
                .Where(s => s.DoctorId == request.DoctorId &&
                            s.Date.Date >= today && s.Date.Date <= tomorrow)
                .OrderBy(s => s.Date)
                .ThenBy(s => s.StartTime)
                .ToListAsync();

            List<SlotPatientResponseDto> slotPatientResponseDtos = slots.Select(s => new SlotPatientResponseDto
            {
               Date =s.Date,
               DoctorId = s.DoctorId,
               EndTime = s.EndTime,
               SlotId = s.SlotId,
               StartTime = s.StartTime,
               Status=s.Status
            }).ToList();
            return slotPatientResponseDtos;
            }
        }
    }

