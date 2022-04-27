using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.FluentMap.Mapping;
using DapperExtensions.Mapper;

namespace API.DAL.UseCases.DutyHoursManagement
{

        [Table("dutyhours")]
        public class DbDutyHours : DbEntity
        {
            [Column("signInBookingIdent")] public Guid SignInBookingIdent { get; set; }
            [Column("signOutBookingIdent")] public Guid SignOutBookingIdent { get; set; }
            [Column("serviceLogTypeId")] public int? ServiceLogTypeId { get; set; }
            [Column("serviceLogDescriptionId")] public int? ServiceLogDescriptionId { get; set; }
        }
    
        public class DbDutyHoursCrudMap : ClassMapper<DbDutyHours>
        {
            public DbDutyHoursCrudMap()
            {
                Table("dutyhours");
                Map(p => p.Ident).Key(KeyType.Assigned);
                Map(p => p.SignInBookingIdent).Column("signInBookingIdent");
                Map(p => p.SignOutBookingIdent).Column("signOutBookingIdent");
                Map(p => p.ServiceLogTypeId).Column("serviceLogTypeId");
                Map(p => p.ServiceLogDescriptionId).Column("serviceLogDescriptionId");
                AutoMap();
            }
        }

        public class DbDutyHoursQueryMap : EntityMap<DbDutyHours>
        {
            public DbDutyHoursQueryMap()
            {
                Map(p => p.Ident).ToColumn("Ident");
                Map(p => p.SignInBookingIdent).ToColumn("signInBookingIdent");
                Map(p => p.SignOutBookingIdent).ToColumn("signOutBookingIdent");
                Map(p => p.ServiceLogTypeId).ToColumn("serviceLogTypeId");
                Map(p => p.ServiceLogDescriptionId).ToColumn("serviceLogDescriptionId");
            }
        }
    
}