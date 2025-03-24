using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AppointmentSystem.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public required string FullName { get; set; }
        [InverseProperty("User")]
        public Doctor Doctor { get; set; }
        [InverseProperty("User")]
        public Patient Patient { get; set; }
    }
}
