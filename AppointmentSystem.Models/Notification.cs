using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Models
{
   public class Notification
    {
        [Key]
        public Guid NotificationId { get; set; } 
        public String UserId { get; set; }
        [Required]
        public String message { get; set; }
        public bool isRead { get; set; } = false;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
