namespace LokalestyringWS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [Key]
        public int User_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string User_Name { get; set; }

        [Required]
        [StringLength(100)]
        public string User_Email { get; set; }

        [Required]
        [StringLength(16)]
        public string Password { get; set; }

        public bool Teacher { get; set; }

        public override string ToString()
        {
            return $"{User_Id}, {User_Name}, {User_Email}, {Teacher}";
        }
    }
}
