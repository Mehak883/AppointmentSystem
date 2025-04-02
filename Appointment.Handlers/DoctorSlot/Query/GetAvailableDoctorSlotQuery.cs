using AppointmentSystem.Dtos.Slot;
using MediatR;


namespace AppointmentSystem.Handlers.DoctorSlot.Query
{
  public  class GetAvailableDoctorSlotQuery:IRequest<List<SlotPatientResponseDto>>
    {
        public int DoctorId { get; set; }
       public GetAvailableDoctorSlotQuery(int doctorId)
        {
            DoctorId = doctorId;
        }
    }
}
