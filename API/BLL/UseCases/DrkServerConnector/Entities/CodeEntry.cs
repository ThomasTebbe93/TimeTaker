using System;
using Newtonsoft.Json;

namespace API.BLL.UseCases.DrkServerConnector.Entities
{
    public class CodeEntry
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("objectHash")] public Guid ObjectHash { get; set; }
        [JsonProperty("permissions")] public EntityPermissions Permissions { get; set; }
        [JsonProperty("listId")] public int ListId { get; set; }
        [JsonProperty("listIdentifier")] public string ListIdentifier { get; set; }
        [JsonProperty("value1")] public string Shortcut { get; set; }
        [JsonProperty("value2")] public string Name { get; set; }
        [JsonProperty("value3")] public string Value3 { get; set; }
        [JsonProperty("value4")] public string Value4 { get; set; }
    }
}