using System.Text.Json.Serialization;
using API.BLL.Base;

namespace API.BLL.UseCases.DutyHoursManagement.Entities
{
    public class DutyHoursSearchOptions : SearchOption
    {
        public string UserName { get; set; }
        public DutyHoursSortColumn? SortColumn { get; set; }
    }


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DutyHoursSortColumn
    {
        Date,
        User
    }
}