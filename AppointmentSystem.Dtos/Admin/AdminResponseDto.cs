using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Dtos.Admin
{
    public class AdminResponseDto
    {

        public required string UserId { get; set; }
        public required string Fullname { get; set; }

        public required string Email { get; set; }
        public required string Role { get; set; }
    
    }
}
