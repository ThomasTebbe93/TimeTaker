using System.Collections.Generic;
using API.BLL.Base;
using API.BLL.UseCases.DrkServerServiceLogTypes.Daos;
using API.BLL.UseCases.DrkServerServiceLogTypes.Entities;

namespace API.BLL.UseCases.DrkServerServiceLogTypes.Services
{
    public interface IServiceLogTypeService
    {
        DataTableSearchResult<ServiceLogType> FindBySearchValue(ServiceLogTypeSearchOptions search);
        List<ServiceLogType> Autocomplete(string searchValue);
    }

    public class ServiceLogTypeService : IServiceLogTypeService
    {
        private readonly IServiceLogTypeDao descriptionDao;

        public ServiceLogTypeService(IServiceLogTypeDao descriptionDao)
        {
            this.descriptionDao = descriptionDao;
        }

        public List<ServiceLogType> Autocomplete(string searchValue) =>
            descriptionDao.GetAllForAutocomplete(searchValue);

        public DataTableSearchResult<ServiceLogType> FindBySearchValue(ServiceLogTypeSearchOptions search)
            => descriptionDao.FindBySearchValue(search);
    }
}