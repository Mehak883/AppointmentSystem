using AppointmentSystem.Dtos.Doctor;
using MediatR;

namespace AppointmentSystem.Handlers.Doctor.Query
{
  public class GetDoctorByUserIdQuery:IRequest<DoctorResponseDto>
    {
        public string UserId { get; set; }
        public GetDoctorByUserIdQuery(string userId) => UserId = userId;
    }
}
