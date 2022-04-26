using API.BLL.Base;
using API.BLL.UseCases.DrkServerServiceLogTypes.Entities;
using API.DAL.UseCases.DrkServerServiceLogTypes;

namespace API.BLL.UseCases.DrkServerServiceLogTypes.Transformer
{
    public class ServiceLogTypeTransformer : ITransformer<ServiceLogType, DbServiceLogType>
    {
        public ServiceLogType ToEntity(DbServiceLogType entity)
        {
            return new ServiceLogType()
            {
                Id = entity.Id,
                ObjectHash = entity.ObjectHash,
                ListId = entity.ListId,
                ListIdentifier = entity.ListIdentifier,
                Shortcut = entity.Shortcut,
                Name = entity.Name,
                Value3 = entity.Value3,
                Value4 = entity.Value4,
            };
        }

        public DbServiceLogType ToDbEntity(ServiceLogType entity)
        {
            return new DbServiceLogType()
            {
                Id = entity.Id,
                ObjectHash = entity.ObjectHash,
                ListId = entity.ListId,
                ListIdentifier = entity.ListIdentifier,
                Shortcut = entity.Shortcut,
                Name = entity.Name,
                Value3 = entity.Value3,
                Value4 = entity.Value4,
            };
        }
    }
}