using System;

namespace MjrChess.Trainer.Models
{
    /// <summary>
    /// General base class for an entity with an ID, created date, and modified date.
    /// </summary>
    public class IEntity
    {
        public int Id { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }
    }
}
