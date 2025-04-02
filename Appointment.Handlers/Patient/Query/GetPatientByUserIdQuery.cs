using AppointmentSystem.Dtos.Patient;
using MediatR;

namespace AppointmentSystem.Handlers.Patient.Query
{
    public class GetPatientByUserIdQuery:IRequest<PatientResponseDto>
    {
        public string UserId { get; set; }
        public GetPatientByUserIdQuery(string id)
        {
            UserId = id;
        }
    }
}
