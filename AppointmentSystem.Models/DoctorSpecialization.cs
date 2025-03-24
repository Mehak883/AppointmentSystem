using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Models
{
   public class DoctorSpecialization
    {
        [Key]
        public int DoctorSpecializationId { get; set; }
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public int SpecializationId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        [ForeignKey("SpecializationId")]
        public Specialization Specialization { get; set; }
    }
}
