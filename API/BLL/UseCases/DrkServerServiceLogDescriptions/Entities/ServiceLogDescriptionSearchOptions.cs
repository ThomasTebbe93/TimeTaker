using System.Text.Json.Serialization;
using API.BLL.Base;

namespace API.BLL.UseCases.DrkServerServiceLogDescriptions.Entities
{
    public class ServiceLogDescriptionSearchOptions : SearchOption
    {
        public string Id { get; set; }
        public string ListId { get; set; }
        public string Shortcut { get; set; }
        public string Name { get; set; }
        public ServiceLogDescriptionSortColumn? SortColumn { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ServiceLogDescriptionSortColumn
    {
        Id,
        ListId,
        Shortcut,
        Name
    }
}