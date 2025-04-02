using AppointmentSystem.Dtos.Appointment;
using AppointmentSystem.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Appointment.Query
{
    public class GetAppointmentsQuery :IRequest<List<AppointmentResponseDto>>
    {
        public int PatientId { get; set; }
    public GetAppointmentsQuery(int patientId) => PatientId = patientId;
}
}
