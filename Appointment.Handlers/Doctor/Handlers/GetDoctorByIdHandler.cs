using AppointmentSystem.Data;
using AppointmentSystem.Dtos.Doctor;
using AppointmentSystem.Dtos.Specialization;
using AppointmentSystem.Handlers.Doctor.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace AppointmentSystem.Handlers.Doctor.Handlers
{
    class GetDoctorByIdHandler : IRequestHandler<GetDoctorByIdQuery, DoctorResponseDto>
    {
        private readonly AppointmentDbContext _context;
        private readonly ILogger<GetDoctorByIdHandler> _logger;
        public GetDoctorByIdHandler(AppointmentDbContext context, ILogger<GetDoctorByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<DoctorResponseDto> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching doctor details for DoctorId: {DoctorId}", request.DoctorId);

            var doctor = await _context.Doctors
            .Include(d => d.User)
            .Include(d => d.DoctorSpecializations)
                .ThenInclude(ds => ds.Specialization)
            .Where(d => d.DoctorId == request.DoctorId)
            .Select(d => new DoctorResponseDto
            {
                DoctorId = d.DoctorId,
                FullName = d.User.FullName,
                StartTime = d.StartTime,
                EndTime = d.EndTime,
                SlotDuration = d.SlotDuration,
                Specializations = d.DoctorSpecializations.Select(ds => new SpecializationResponseDto { SpecializationName = ds.Specialization.SpecializationName, SpecializationId = ds.SpecializationId }).ToList()

            })
            .FirstOrDefaultAsync(cancellationToken);

            if (doctor == null)
            {
                _logger.LogWarning("Doctor with ID {DoctorId} not found", request.DoctorId);
                throw new Exception("Doctor not found");
            }
            _logger.LogInformation("Successfully fetched doctor details for DoctorId: {DoctorId}", request.DoctorId);
            return doctor;



        }
    }
}
