using System;
using System.ComponentModel.DataAnnotations;
using API.BLL.Base;
using API.BLL.UseCases.Memberships.Entities;

namespace API.BLL.UseCases.DutyHoursManagement.Entities
{
    public class DutyHoursBooking
        {
            [Key] public DutyHoursBookingIdent Ident { get; set; }
            public UserIdent UserIdent { get; set; }
            public UserIdent CreatorIdent { get; set; }
            public User User { get; set; }
            public User Creator { get; set; }
            public DateTimeOffset BookingTime { get; set; }
            public bool IsSignedIn { get; set; }
        
        public DutyHoursBooking(){}

        public DutyHoursBooking(DutyHoursBooking entity)
        {
            Ident = entity.Ident;
            UserIdent = entity.UserIdent;
            User = entity.User;
            CreatorIdent = entity.CreatorIdent;
            Creator = entity.Creator;
            BookingTime = entity.BookingTime;
            IsSignedIn = entity.IsSignedIn;
        }
    }
    
    public class DutyHoursBookingIdent : IdentBase
    {
        public DutyHoursBookingIdent(Guid ident) : base(ident)
        {
        }
    }
}