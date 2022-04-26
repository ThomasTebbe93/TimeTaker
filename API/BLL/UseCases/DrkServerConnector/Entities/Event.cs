using System;
using Newtonsoft.Json;

namespace API.BLL.UseCases.DrkServerConnector.Entities
{
    public class Event
    {
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("objectHash")] public Guid ObjectHash { get; set; }
        [JsonProperty("permissions")] public EntityPermissions Permissions { get; set; }

        [JsonProperty("organisation")] public Organisation Organisation { get; set; }
        [JsonProperty("organizer")] public ComplexAddressContact Organizer { get; set; }
        [JsonProperty("dateFrom")] public DateTimeOffset DateFrom { get; set; }
        [JsonProperty("dateUpTo")] public DateTimeOffset DateUpTo { get; set; }

        [JsonProperty("category")] public CodeEntry Category { get; set; }
        [JsonProperty("apprenticeshipType")] public ApprenticeshipType ApprenticeshipType { get; set; }
        [JsonProperty("description")] public CodeEntry Description { get; set; }

        [JsonProperty("extendedDescription")] public string ExtendedDescription { get; set; }
        [JsonProperty("countOfInstruction")] public int CountOfInstruction { get; set; }
        [JsonProperty("minutesPerInstruction")] public int? MinutesPerInstruction { get; set; }
    }
}