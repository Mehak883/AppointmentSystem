using AppointmentSystem.Data;
using AppointmentSystem.Dtos.Doctor;
using AppointmentSystem.Dtos.Specialization;
using AppointmentSystem.Handlers.Doctor.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Doctor.Handlers
{
    class GetDoctorByIdHandler:IRequestHandler<GetDoctorByIdQuery,DoctorResponseDto>
    {
        private readonly AppointmentDbContext _context;

        public GetDoctorByIdHandler(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<DoctorResponseDto> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
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

            if (doctor == null) throw new Exception("Doctor not found");
            return doctor;
        }

    }
}
