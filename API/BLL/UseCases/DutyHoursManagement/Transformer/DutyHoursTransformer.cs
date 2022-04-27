using API.BLL.Base;
using API.BLL.Extensions;
using API.BLL.UseCases.DutyHoursManagement.Entities;
using API.DAL.UseCases.DutyHoursManagement;

namespace API.BLL.UseCases.DutyHoursManagement.Transformer
{
    public class DutyHoursTransformer : ITransformer<DutyHours, DbDutyHours>
    {
        public DutyHours ToEntity(DbDutyHours entity)
        {
            return new DutyHours()
            {
                Ident = entity.Ident.Ident<DutyHoursIdent>(),
                SignInBookingIdent = entity.SignInBookingIdent.Ident<DutyHoursBookingIdent>(),
                SignOutBookingIdent = entity.SignOutBookingIdent.Ident<DutyHoursBookingIdent>(),
                ServiceLogTypeId = entity.ServiceLogTypeId,
                ServiceLogDescriptionId = entity.ServiceLogDescriptionId
            };
        }

        public DbDutyHours ToDbEntity(DutyHours entity)
        {
            return new DbDutyHours()
            {
                Ident = entity.Ident.Ident,
                SignInBookingIdent = entity.SignInBookingIdent.Ident,
                SignOutBookingIdent = entity.SignOutBookingIdent.Ident,
                ServiceLogDescriptionId = entity.ServiceLogDescriptionId,
                ServiceLogTypeId = entity.ServiceLogTypeId,
            };
        }
    }
}