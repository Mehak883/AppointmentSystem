using AppointmentSystem.Data;
using AppointmentSystem.Dtos.Doctor;
using AppointmentSystem.Dtos.Specialization;
using AppointmentSystem.Handlers.Doctor.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace AppointmentSystem.Handlers.Doctor.Handlers
{
    class GetDoctorByUserIdHandler : IRequestHandler<GetDoctorByUserIdQuery,DoctorResponseDto>
    {
        private readonly AppointmentDbContext _context;

        public GetDoctorByUserIdHandler(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<DoctorResponseDto> Handle(GetDoctorByUserIdQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _context.Doctors
                 .Include(d => d.User)
                .Include(d => d.DoctorSpecializations)
                    .ThenInclude(ds => ds.Specialization)
                .FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);
            DoctorResponseDto doctorResponseDto = new DoctorResponseDto
            {
                DoctorId = doctor.DoctorId,
                EndTime = doctor.EndTime,
                FullName = doctor.User.FullName,
                SlotDuration = doctor.SlotDuration,
                StartTime = doctor.StartTime,
                Specializations = doctor.DoctorSpecializations.Select(ds => new SpecializationResponseDto { SpecializationName = ds.Specialization.SpecializationName, SpecializationId = ds.SpecializationId }).ToList()


            };
            return doctorResponseDto;
        }
    }
}
