using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Doctor.Command
{
   public class DeleteDoctorRequest:IRequest<bool>
    {
        public int DoctorId { get; set; }
    }
}
