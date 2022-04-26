using System.Collections.Generic;
using API.BLL.Base;
using API.BLL.UseCases.DrkServerServiceLogTypes.Entities;

namespace API.BLL.UseCases.DrkServerServiceLogTypes.Daos
{
    public interface IServiceLogTypeDao
    {
        bool DeleteAll();
        void CreateMany(List<ServiceLogType> entities);
        DataTableSearchResult<ServiceLogType> FindBySearchValue(ServiceLogTypeSearchOptions searchOptions);
    }
}