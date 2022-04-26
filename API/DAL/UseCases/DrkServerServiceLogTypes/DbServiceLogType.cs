using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.FluentMap.Mapping;
using DapperExtensions.Mapper;

namespace API.DAL.UseCases.DrkServerServiceLogTypes
{
    [Table("servicelogtype")]
    public class DbServiceLogType
    {
        public int Id { get; set; }
        public Guid ObjectHash { get; set; }
        public int ListId { get; set; }
        public string ListIdentifier { get; set; }
        public string Shortcut { get; set; }
        public string Name { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
    }

    public class DbServiceLogTypeCrudMap : ClassMapper<DbServiceLogType>
    {
        public DbServiceLogTypeCrudMap()
        {
            Table("servicelogtype");
            Map(p => p.Id).Key(KeyType.Assigned);
            Map(p => p.ObjectHash).Column("ObjectHash");
            Map(p => p.ListId).Column("ListId");
            Map(p => p.ListIdentifier).Column("ListIdentifier");
            Map(p => p.Shortcut).Column("Shortcut");
            Map(p => p.Name).Column("Name");
            Map(p => p.Value3).Column("Value3");
            Map(p => p.Value4).Column("Value4");
            AutoMap();
        }
    }

    public class DbServiceLogTypeQueryMap : EntityMap<DbServiceLogType>
    {
        public DbServiceLogTypeQueryMap()
        {
            Map(p => p.Id).ToColumn("Id");
            Map(p => p.ObjectHash).ToColumn("ObjectHash");
            Map(p => p.ListId).ToColumn("ListId");
            Map(p => p.ListIdentifier).ToColumn("ListIdentifier");
            Map(p => p.Shortcut).ToColumn("Shortcut");
            Map(p => p.Name).ToColumn("Name");
            Map(p => p.Value3).ToColumn("Value3");
            Map(p => p.Value4).ToColumn("Value4");
        }
    }
}