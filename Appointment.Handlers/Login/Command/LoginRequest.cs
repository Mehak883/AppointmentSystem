using MediatR;

namespace AppointmentSystem.Handlers.Login.Command
{
    public class LoginRequest: IRequest<bool>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    
}
}
