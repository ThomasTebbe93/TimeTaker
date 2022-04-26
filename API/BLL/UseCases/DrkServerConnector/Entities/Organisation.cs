using System;
using Newtonsoft.Json;

namespace API.BLL.UseCases.DrkServerConnector.Entities
{
    public class Organisation
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("objectHash")] public Guid ObjectHash { get; set; }
        [JsonProperty("permissions")] public EntityPermissions Permissions { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
    }
}