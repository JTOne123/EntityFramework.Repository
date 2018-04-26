namespace Samples.Common.CodeFirst
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Posts_hashtags
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public int HashtagId { get; set; }

        public virtual Hashtag Hashtag { get; set; }

        public virtual Post Post { get; set; }
    }
}
