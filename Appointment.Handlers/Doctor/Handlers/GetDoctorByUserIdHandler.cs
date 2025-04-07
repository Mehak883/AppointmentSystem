using AppointmentSystem.Data;
using AppointmentSystem.Dtos.Doctor;
using AppointmentSystem.Dtos.Specialization;
using AppointmentSystem.Handlers.Doctor.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace AppointmentSystem.Handlers.Doctor.Handlers
{
    class GetDoctorByUserIdHandler : IRequestHandler<GetDoctorByUserIdQuery,DoctorResponseDto>
    {
        private readonly AppointmentDbContext _context;
        private readonly ILogger<GetDoctorByUserIdHandler> _logger;
        public GetDoctorByUserIdHandler(AppointmentDbContext context, ILogger<GetDoctorByUserIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<DoctorResponseDto> Handle(GetDoctorByUserIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching doctor for UserId: {UserId}", request.UserId);
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
            _logger.LogInformation("Doctor retrieved successfully for UserId: {UserId}", request.UserId);
            return doctorResponseDto;
        }
    }
}
