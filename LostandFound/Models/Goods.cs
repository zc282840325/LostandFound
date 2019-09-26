namespace LostandFound.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Goods
    {
        [Key]
        public int GID { get; set; }

        public int TID { get; set; }

        [Required]
        [StringLength(100)]
        public string GoodsName { get; set; }

        [Required]
        [StringLength(50)]
        public string GoodsImage { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}")]
        public DateTime? Date { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public virtual ItemType ItemType { get; set; }
    }
}
