using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.DoctorSlot.Command
{
   public class BookSlotRequest : IRequest<bool>
    {
        public int SlotId { get; set; }
        public int PatientId { get; set; }
        public int SpecializationId { get; set; }

        public BookSlotRequest(int slotId, int patientId,int specializationId)
        {
            SlotId = slotId;
            PatientId = patientId;
            SpecializationId = specializationId;
        }
    }
}
