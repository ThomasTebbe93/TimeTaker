using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.FluentMap.Mapping;
using DapperExtensions.Mapper;

namespace API.DAL.UseCases.DutyHoursManagement
{
    [Table("dutyhoursbookings")]
    public class DbDutyHoursBooking : DbEntity
    {
        [Column("userident")] public Guid UserIdent { get; set; }
        [Column("creatorident")] public Guid CreatorIdent { get; set; }
        [Column("bookingtime")] public DateTimeOffset BookingTime { get; set; }
        [Column("issignedin")] public bool IsSignedIn { get; set; }
    }
    
    public class DbDutyHoursBookingCrudMap : ClassMapper<DbDutyHoursBooking>
    {
        public DbDutyHoursBookingCrudMap()
        {
            Table("dutyhoursbookings");
            Map(p => p.Ident).Key(KeyType.Assigned);
            Map(p => p.UserIdent).Column("userident");
            Map(p => p.CreatorIdent).Column("creatorident");
            Map(p => p.BookingTime).Column("bookingtime");
            Map(p => p.IsSignedIn).Column("issignedin");
            AutoMap();
        }
    }

    public class DbDutyHoursBookingQueryMap : EntityMap<DbDutyHoursBooking>
    {
        public DbDutyHoursBookingQueryMap()
        {
            Map(p => p.Ident).ToColumn("Ident");
            Map(p => p.UserIdent).ToColumn("userident");
            Map(p => p.CreatorIdent).ToColumn("creatorident");
            Map(p => p.BookingTime).ToColumn("bookingtime");
            Map(p => p.IsSignedIn).ToColumn("issignedin");
        }
    }
}