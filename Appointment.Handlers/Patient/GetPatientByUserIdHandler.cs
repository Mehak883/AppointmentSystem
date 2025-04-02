using AppointmentSystem.Data;
using AppointmentSystem.Dtos.Doctor;
using AppointmentSystem.Dtos.Patient;
using AppointmentSystem.Dtos.Specialization;
using AppointmentSystem.Handlers.Patient.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Patient
{
    class GetPatientByUserIdHandler : IRequestHandler<GetPatientByUserIdQuery, PatientResponseDto>
    {
        private readonly AppointmentDbContext _context;

        public GetPatientByUserIdHandler(AppointmentDbContext context)
        {
            _context = context;
        }
        public async Task<PatientResponseDto> Handle(GetPatientByUserIdQuery request, CancellationToken cancellationToken)
        {

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);
            PatientResponseDto PatientResponseDto = new PatientResponseDto
            {
               PatientId =patient.PatientId,
               Gender= patient.Gender,
               UserId=patient.UserId
                

            };
            return PatientResponseDto;
        }
    }
}
