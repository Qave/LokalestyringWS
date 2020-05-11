namespace LokalestyringWS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Room")]
    public partial class Room
    {
        [Key]
        public int Room_Id { get; set; }

        public int Floor { get; set; }

        [Required]
        [StringLength(5)]
        public string No { get; set; }

        public int Type_Id { get; set; }

        public int Building_Id { get; set; }

        public int Loc_Id { get; set; }

        public virtual Building Building { get; set; }

        public virtual Location Location { get; set; }

        public virtual Roomtype Roomtype { get; set; }

        public override string ToString()
        {
            return $"{Room_Id}, {Floor}, {No}, {Type_Id}, {Building_Id}, {Loc_Id}";
        }

    }
}
