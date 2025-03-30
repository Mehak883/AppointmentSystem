using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Dtos.Specialization
{
   public class SpecializationRequestDto
    {
        [Required]
        public required string SpecializationName { get; set; }
    }
}
