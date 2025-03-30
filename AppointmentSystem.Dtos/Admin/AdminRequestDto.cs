using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Dtos.Admin
{
    public class AdminRequestDto
    {
            [Required(ErrorMessage = "Name is required")]
            [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Name must contain only letters and spaces.")]
            public required string FullName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public required string Email { get; set; }
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
            public required string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Confirm Password must match Password.")]
        public required string ConfirmPassword { get; set; }
    }
}
