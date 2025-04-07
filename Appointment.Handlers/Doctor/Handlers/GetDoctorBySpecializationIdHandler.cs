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
    class GetDoctorBySpecializationIdHandler:IRequestHandler<GetDoctorsBySpecializationIdQuery,List<DoctorResponseDto>>
    {
        private readonly AppointmentDbContext _context;
        private readonly ILogger<GetDoctorBySpecializationIdHandler> _logger;
        public GetDoctorBySpecializationIdHandler(AppointmentDbContext context, ILogger<GetDoctorBySpecializationIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<DoctorResponseDto>> Handle(GetDoctorsBySpecializationIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching doctors for SpecializationId: {SpecializationId}", request.SpecializationId);
            var doctors= await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.DoctorSpecializations)
                    .ThenInclude(ds => ds.Specialization)
                .Where(d => d.DoctorSpecializations.Any(ds => ds.SpecializationId == request.SpecializationId))
                .Select(d => new DoctorResponseDto
                {
                    DoctorId = d.DoctorId,
                    FullName = d.User.FullName,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                    SlotDuration = d.SlotDuration,
                    Specializations = d.DoctorSpecializations.Select(ds => new SpecializationResponseDto { SpecializationName = ds.Specialization.SpecializationName ,SpecializationId = ds.SpecializationId}).ToList()
                })
                .ToListAsync(cancellationToken);
            _logger.LogInformation("Retrieved {Count} doctors for SpecializationId: {SpecializationId}", doctors.Count, request.SpecializationId);
            return doctors;
        }
    }
}
