using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Appointment.Command
{
  
    public class CancelAppointmentRequest : IRequest<bool>
    {
        public int AppointmentId { get; set; }
        public CancelAppointmentRequest(int appointmentId) => AppointmentId = appointmentId;
    }
}
