using System.Collections.Generic;
using API.BLL.Base;
using API.BLL.UseCases.DrkServerServiceLogDescriptions.Entities;

namespace API.BLL.UseCases.DrkServerServiceLogDescriptions.Daos
{
    public interface IServiceLogDescriptionDao
    {
        bool DeleteAll();
        void CreateMany(List<ServiceLogDescription> entities);
        DataTableSearchResult<ServiceLogDescription> FindBySearchValue(ServiceLogDescriptionSearchOptions searchOptions);
    }
}