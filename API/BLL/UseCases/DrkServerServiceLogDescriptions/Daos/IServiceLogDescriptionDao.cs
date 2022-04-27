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
        List<ServiceLogDescription> GetAllForAutocomplete(string searchValue);
        List<ServiceLogDescription> FindByIds(HashSet<int> ids);
    }
}