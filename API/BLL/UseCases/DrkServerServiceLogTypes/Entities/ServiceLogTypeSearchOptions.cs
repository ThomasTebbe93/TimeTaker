using System.Text.Json.Serialization;
using API.BLL.Base;

namespace API.BLL.UseCases.DrkServerServiceLogTypes.Entities
{
    public class ServiceLogTypeSearchOptions : SearchOption
    {
        public string Id { get; set; }
        public string ListId { get; set; }
        public string Shortcut { get; set; }
        public string Name { get; set; }
        public ServiceLogTypeSortColumn? SortColumn { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ServiceLogTypeSortColumn
    {
        Id,
        ListId,
        Shortcut,
        Name
    }
}