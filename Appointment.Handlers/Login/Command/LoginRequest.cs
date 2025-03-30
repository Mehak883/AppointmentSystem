using AppointmentSystem.Dtos.Admin;
using MediatR;

namespace AppointmentSystem.Handlers.Login.Command
{
    public class LoginRequest: IRequest<AdminResponseDto>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    
}
}
