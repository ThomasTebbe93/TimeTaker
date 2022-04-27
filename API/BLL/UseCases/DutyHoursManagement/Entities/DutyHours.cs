using System;
using System.ComponentModel.DataAnnotations;
using API.BLL.Base;
using API.BLL.UseCases.DrkServerServiceLogDescriptions.Entities;
using API.BLL.UseCases.DrkServerServiceLogTypes.Entities;

namespace API.BLL.UseCases.DutyHoursManagement.Entities
{
    public class DutyHours
    {
        [Key] public DutyHoursIdent Ident { get; set; }
        public DutyHoursBookingIdent SignInBookingIdent { get; set; }
        public DutyHoursBookingIdent SignOutBookingIdent { get; set; }
        public DutyHoursBooking SignInBooking { get; set; }
        public DutyHoursBooking SignOutBooking { get; set; }
        public ServiceLogType ServiceLogType { get; set; }
        public int? ServiceLogTypeId { get; set; }
        public ServiceLogDescription ServiceLogDescription { get; set; }
        public int? ServiceLogDescriptionId { get; set; }
        
        public DutyHours(){}

        public DutyHours(DutyHours entity)
        {
            Ident = entity.Ident;
            SignInBookingIdent = entity.SignInBookingIdent;
            SignOutBookingIdent = entity.SignOutBookingIdent;
            SignInBooking = entity.SignInBooking;
            SignOutBooking = entity.SignOutBooking;
            ServiceLogType = entity.ServiceLogType;
            ServiceLogTypeId = entity.ServiceLogTypeId;
            ServiceLogDescription = entity.ServiceLogDescription;
            ServiceLogDescriptionId = entity.ServiceLogDescriptionId;
        }
    }
    
    public class DutyHoursIdent : IdentBase
    {
        public DutyHoursIdent(Guid ident) : base(ident)
        {
        }
    }
}