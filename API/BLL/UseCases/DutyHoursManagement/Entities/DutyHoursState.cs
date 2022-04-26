using System.Collections.Generic;

namespace API.BLL.UseCases.DutyHoursManagement.Entities
{
    public class DutyHoursState
    {
        public List<DutyHours> Hours { get; set; }
        public DutyHoursBooking Booking { get; set; }

        public DutyHoursState()
        {
        }
    }
}