namespace AppointmentSystem.Models
{
    public enum SlotStatus
    {
        Available,
        Booked,
        Completed,
        Cancelled
    }
    public enum LeaveRequestType
    {
        Full,
        Half
    }
    public enum HalfLeaveRequestType
    {
        First,
        Second
    }
    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }
    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
