namespace Samples.Common.CodeFirst
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
            Posts_hashtags = new HashSet<Posts_hashtags>();
        }

        public int Id { get; set; }

        [Column("Post", TypeName = "text")]
        [Required]
        public string Post1 { get; set; }

        public DateTime? DatePosted { get; set; }

        public int CategoryId { get; set; }

        [Column(TypeName = "date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DayPosted { get; set; }

        [Required]
        [StringLength(64)]
        public string Topic { get; set; }

        public int AutorId { get; set; }

        public virtual PostCategory PostCategory { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Posts_hashtags> Posts_hashtags { get; set; }
    }
}
