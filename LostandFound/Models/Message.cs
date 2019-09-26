namespace LostandFound.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Message")]
    public partial class Message
    {
        [Key]
        public int MID { get; set; }

        public int UID { get; set; }

        [Column("Message")]
        [Required]
        [StringLength(50)]
        public string Message1 { get; set; }
    }
}
