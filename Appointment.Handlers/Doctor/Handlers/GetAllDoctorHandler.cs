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
    class GetAllDoctorHandler:IRequestHandler<GetAllDoctorsQuery,List<DoctorResponseDto>>
    {
        private readonly AppointmentDbContext _context;
        private readonly ILogger<GetAllDoctorHandler> _logger;
        public GetAllDoctorHandler(AppointmentDbContext context, ILogger<GetAllDoctorHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<DoctorResponseDto>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all doctors...");
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.DoctorSpecializations)
                    .ThenInclude(ds => ds.Specialization)
                .Select(d => new DoctorResponseDto
                {
                    DoctorId = d.DoctorId,
                    FullName = d.User.FullName,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                    SlotDuration = d.SlotDuration,
                    Specializations = d.DoctorSpecializations.Select(ds => new SpecializationResponseDto { SpecializationName = ds.Specialization.SpecializationName, SpecializationId = ds.SpecializationId }).ToList()

                })
                .ToListAsync(cancellationToken);
        }
    }
}
