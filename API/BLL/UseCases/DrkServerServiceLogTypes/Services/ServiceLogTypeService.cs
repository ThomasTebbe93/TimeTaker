using API.BLL.Base;
using API.BLL.UseCases.DrkServerServiceLogTypes.Daos;
using API.BLL.UseCases.DrkServerServiceLogTypes.Entities;

namespace API.BLL.UseCases.DrkServerServiceLogTypes.Services
{
    public interface IServiceLogTypeService
    {
        DataTableSearchResult<ServiceLogType> FindBySearchValue(ServiceLogTypeSearchOptions search);
    }

    public class ServiceLogTypeService : IServiceLogTypeService
    {
        private readonly IServiceLogTypeDao descriptionDao;

        public ServiceLogTypeService(IServiceLogTypeDao descriptionDao)
        {
            this.descriptionDao = descriptionDao;
        }


        public DataTableSearchResult<ServiceLogType> FindBySearchValue(ServiceLogTypeSearchOptions search)
            => descriptionDao.FindBySearchValue(search);
    }
}