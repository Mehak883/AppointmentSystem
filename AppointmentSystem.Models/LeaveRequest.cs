using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentSystem.Models { 

public class LeaveRequest
{
    [Key]
    public int LeaveRequestId { get; set; }

    [Required]
    public int DoctorId { get; set; }

    [ForeignKey("DoctorId")]
    public Doctor Doctor { get; set; }
    [Required]
    [StringLength(255)]
    public string Reason { get; set; }
    [Required]
    public LeaveRequestType LeaveType { get; set; }
    public HalfLeaveRequestType? HalfLeaveType { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Required]
    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
}
   
}
