using AppointmentSystem.Dtos.Doctor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Doctor.Query
{
    public class GetDoctorsBySpecializationIdQuery : IRequest<List<DoctorResponseDto>>
    {
        public int SpecializationId { get; set; }

        public GetDoctorsBySpecializationIdQuery(int specializationId)
        {
            SpecializationId = specializationId;
        }
    }
}
