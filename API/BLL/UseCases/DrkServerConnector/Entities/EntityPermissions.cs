using Newtonsoft.Json;

namespace API.BLL.UseCases.DrkServerConnector.Entities
{
    public class EntityPermissions
    {
        [JsonProperty("read")] public bool Read { get; set; }
        [JsonProperty("create")] public bool Create { get; set; }
        [JsonProperty("update")] public bool Update { get; set; }
        [JsonProperty("delete")] public bool Delete { get; set; }
        [JsonProperty("yesNo")] public bool YesNo { get; set; }
    }
}