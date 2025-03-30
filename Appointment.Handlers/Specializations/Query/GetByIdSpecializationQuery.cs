using AppointmentSystem.Dtos.Specialization;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Specializations.Query
{
    public class GetByIdSpecializationQuery : IRequest<SpecializationResponseDto>
    {
        public int SpecializationId { get; set; }
    }
}
