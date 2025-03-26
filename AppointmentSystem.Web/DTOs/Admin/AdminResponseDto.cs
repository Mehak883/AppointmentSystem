using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Web.DTOs.Admin
{
    public class AdminResponseDto
    {

        public required string UserId;
        //[Required(ErrorMessage = "Name is required")]
        //[RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Name must contain only letters and spaces.")]
        public required string Fullname { get; set; }
        //[Required(ErrorMessage = "Email is required.")]
        //[EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }
        //[EmailAddress(ErrorMessage = "Invalid email format.")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
        //    ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        //public required string Password { get; set; }
    }
}
