using System;

namespace API.BLL.UseCases.DutyHoursManagement.Entities
{
    public class ChipSingEntity
    {
        public Guid UserIdent { get; set; }
        public DateTime BookingTime { get; set; }
        
        public ChipSingEntity(){}

        public ChipSingEntity(ChipSingEntity existing)
        {
            UserIdent = existing.UserIdent;
            BookingTime = existing.BookingTime;
        }
    }
}