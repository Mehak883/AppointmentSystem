using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Models
{
   public class Slot
    {
        [Key]
        public int SlotId;
        [Required]
        public int DoctorId;
        [Required]
       
        public DateTime date;
        [Required]
        [DataType(DataType.Time)]
        public DateTime startTime;
        [Required]
        [DataType(DataType.Time)]
        public DateTime EndTime;
        [Required]
        public SlotStatus StatusId = SlotStatus.Available;

    }
}
