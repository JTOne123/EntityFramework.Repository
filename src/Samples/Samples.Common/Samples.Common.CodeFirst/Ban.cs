namespace Samples.Common.CodeFirst
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ban
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int? ModeratorId { get; set; }

        public DateTime DateBanned { get; set; }

        public int DaysBanned { get; set; }

        public virtual Moderator Moderator { get; set; }

        public virtual User User { get; set; }
    }
}
