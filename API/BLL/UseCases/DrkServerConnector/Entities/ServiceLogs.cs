using System.Collections.Generic;
using Newtonsoft.Json;

namespace API.BLL.UseCases.DrkServerConnector.Entities
{
    public class ServiceLogs
    {
        [JsonProperty("totalItems")] public int TotalItems { get; set; }
        [JsonProperty("offset")] public int Offset { get; set; }
        [JsonProperty("limit")] public int Limit { get; set; }
        [JsonProperty("totalActivityLength")] public int TotalActivityLength { get; set; }

        [JsonProperty("totalActivityLengthHours")]
        public double? TotalActivityLengthHours { get; set; }

        [JsonProperty("totalActivityLengthHoursString")]
        public string? TotalActivityLengthHoursString { get; set; }

        [JsonProperty("items")] public List<ServiceLog>? Items { get; set; }
    }
}