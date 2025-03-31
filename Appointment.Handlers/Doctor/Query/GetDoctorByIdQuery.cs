using AppointmentSystem.Dtos.Doctor;
using MediatR;


namespace AppointmentSystem.Handlers.Doctor.Query
{
    public class GetDoctorByIdQuery : IRequest<DoctorResponseDto>
    {
        public int DoctorId { get; set; }

}
}
