namespace LostandFound.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [Key]
        public int UID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [DisplayName("PassWord")]
        [Required(ErrorMessage = "UserPassWord is required")]
        [StringLength(50)]
        public string UserPassWord { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", ErrorMessage = "Please enter the correct email format£¡")]
        [StringLength(50)]

        public string Email { get; set; }

        [Required(ErrorMessage = "StudentID is required")]
        [StringLength(50)]
        public string StudentID { get; set; }

        [StringLength(100)]
        public string Que1 { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Ans1 is required")]
        public string Ans1 { get; set; }

        [StringLength(100)]
        public string Que2 { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Ans2 is required")]
        public string Ans2 { get; set; }

        [StringLength(250)]
        public string Message { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; }
        [StringLength(100)]
        public string Status { get; set; }
    }
}
