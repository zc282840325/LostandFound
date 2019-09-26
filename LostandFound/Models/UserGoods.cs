namespace LostandFound.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserGoods
    {
        [Key]
        public int OID { get; set; }

        public int UID { get; set; }

        public int? GID { get; set; }

        [StringLength(10)]
        public string Type { get; set; }
    }
    [NotMapped]
    public class UserGoodsOut: UserGoods
    {
        public string GoodName { get; set; }
    }
}
