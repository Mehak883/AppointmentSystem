using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Specializations.Command
{
    public class DeleteSpecializationRequest : IRequest<bool>
    {
        public int SpecializationId { get; set; }
    }
}
