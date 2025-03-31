using AppointmentSystem.Dtos.Doctor;
using MediatR;

namespace AppointmentSystem.Handlers.Doctor.Query
{
    public class GetAllDoctorsQuery : IRequest<List<DoctorResponseDto>>
    {
        
    }
}
