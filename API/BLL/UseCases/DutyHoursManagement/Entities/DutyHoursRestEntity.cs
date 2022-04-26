using System;

namespace API.BLL.UseCases.DutyHoursManagement.Entities
{
    public class DutyHoursRestEntity
    {
        public Guid? Ident { get; set; }
        public int? CustomerId { get; set; }
        public bool? Deleted { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }

        public DutyHoursRestEntity()
        {
        }

        public DutyHoursRestEntity(DutyHoursRestEntity entity)
        {
            Ident = entity.Ident;
            CustomerId = entity.CustomerId;
            Deleted = entity.Deleted;
            Start = entity.Start;
            End = entity.End;
        }
    }
}