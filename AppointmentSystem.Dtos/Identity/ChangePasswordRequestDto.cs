using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Dtos.Identity
{
   public class ChangePasswordRequestDto
    {
        [Required(ErrorMessage ="Current Password is required")]
        public required string OldPassword { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
           ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public required string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Confirm Password must match Password.")]
        public required string ConfirmPassword { get; set; }

    }
}
