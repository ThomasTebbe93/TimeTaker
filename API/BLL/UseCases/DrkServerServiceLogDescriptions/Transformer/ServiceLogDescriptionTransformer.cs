using API.BLL.Base;
using API.BLL.UseCases.DrkServerServiceLogDescriptions.Entities;
using API.DAL.UseCases.DrkServerServiceLogDescriptions;

namespace API.BLL.UseCases.DrkServerServiceLogDescriptions.Transformer
{
    public class ServiceLogDescriptionTransformer : ITransformer<ServiceLogDescription, DbServiceLogDescription>
        {
            public ServiceLogDescription ToEntity(DbServiceLogDescription entity)
            {
                return new ServiceLogDescription()
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

            public DbServiceLogDescription ToDbEntity(ServiceLogDescription entity)
            {
                return new DbServiceLogDescription()
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