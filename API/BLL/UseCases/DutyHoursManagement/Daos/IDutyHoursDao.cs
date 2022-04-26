using System.Collections.Generic;
using API.BLL.Base;
using API.BLL.Helper;
using API.BLL.UseCases.DutyHoursManagement.Entities;
using API.BLL.UseCases.DutyHoursManagement.Transformer;

namespace API.BLL.UseCases.DutyHoursManagement.Daos
{
    public interface IDutyHoursDao : IDao<DutyHours, DutyHoursIdent, DutyHoursTransformer>
    {
        public void Create(DutyHours entity);
        public DataTableSearchResult<DutyHours> FindBySearchValue(Context context, DutyHoursSearchOptions searchOptions);
        public List<DutyHours> GetPersonalBookings(Context context);
    }
}