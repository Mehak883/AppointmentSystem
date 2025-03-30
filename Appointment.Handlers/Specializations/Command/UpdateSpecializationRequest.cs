using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Specializations.Command
{
   public class UpdateSpecializationRequest:IRequest<bool>
    {
        public int SpecializationId { get; set; }
        public required string SpecializationName { get; set; }
    }
}
