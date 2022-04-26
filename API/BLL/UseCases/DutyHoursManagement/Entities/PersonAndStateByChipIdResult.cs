using API.BLL.UseCases.Memberships.Entities;

namespace API.BLL.UseCases.DutyHoursManagement.Entities
{
    public class PersonAndStateByChipIdResult
    {
        public User User { get; set; }
        public DutyHoursBooking LastBooking { get; set; }
        
        public PersonAndStateByChipIdResult(){}
    }
}