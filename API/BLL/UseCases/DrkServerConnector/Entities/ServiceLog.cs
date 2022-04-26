using System;
using Newtonsoft.Json;

namespace API.BLL.UseCases.DrkServerConnector.Entities
{
    public class ServiceLog
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("objectHash")] public Guid ObjectHash { get; set; }
        [JsonProperty("permissions")] public EntityPermissions Permissions { get; set; }
        [JsonProperty("organisation")] public Organisation Organisation { get; set; }

        [JsonProperty("dependendOrganisation")]
        public Organisation dependendOrganisation { get; set; }

        [JsonProperty("type")] public CodeEntry type { get; set; }
        [JsonProperty("description")] public CodeEntry description { get; set; }
        [JsonProperty("extendedDescription")] public string extendedDescription { get; set; }

        [JsonProperty("event")] public Event Event { get; set; }
        [JsonProperty("location")] public ComplexAddressContact location { get; set; }
        [JsonProperty("dateFrom")] public DateTimeOffset dateFrom { get; set; }
        [JsonProperty("dateUpTo")] public DateTimeOffset dateUpTo { get; set; }
        [JsonProperty("timeLogMultiplicator")] public double? timeLogMultiplicator { get; set; }

        [JsonProperty("minutesPerInstruction")]
        public int? minutesPerInstruction { get; set; }

        [JsonProperty("activityLength")] public int activityLength { get; set; }
        [JsonProperty("activityLengthHours")] public double? activityLengthHours { get; set; }

        [JsonProperty("activityLengthHoursString")]
        public string activityLengthHoursString { get; set; }

        [JsonProperty("remark")] public string remark { get; set; }
    }
}