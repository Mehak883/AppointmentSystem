using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Dtos.Admin
{
   public class AdminUpdateDto
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Name must contain only letters and spaces.")]
        public required string FullName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }

    }
}
