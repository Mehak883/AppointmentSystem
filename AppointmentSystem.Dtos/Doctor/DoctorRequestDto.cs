using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Dtos.Doctor
{
   public class DoctorRequestDto
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

        [Required(ErrorMessage = "Shift start time is required")]
        //[Range(typeof(TimeSpan), "00:00:00", "23:59:59", ErrorMessage = "Start time must be a valid time.")]

        public TimeSpan StartTime { get; set; }
        [Required(ErrorMessage = "Shift end time is required.")]
        //[Range(typeof(TimeSpan), "00:00:00", "23:59:59", ErrorMessage = "End time must be a valid time.")]
        public TimeSpan EndTime { get; set; }
        [Required(ErrorMessage = "Slot duration is required.")]
        [Range(15, 60, ErrorMessage = "Slot duration must be between 15 and 60 minutes.")]
        public int SlotDuration { get; set; }
        [Required(ErrorMessage = "At least one specialization is required.")]
        [MinLength(1, ErrorMessage = "At least one specialization must be selected.")]
        public List<int> SpecializationIds { get; set; } = new();

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (EndTime <= StartTime)
        //    {
        //        yield return new ValidationResult("End time must be greater than start time.", new[] { "EndTime" });
        //    }
        //}
    }
}
