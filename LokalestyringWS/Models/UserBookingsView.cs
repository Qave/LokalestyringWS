namespace LokalestyringWS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserBookingsView")]
    public partial class UserBookingsView
    {
        [StringLength(58)]
        public string RoomName { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string City { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string Building_Letter { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Floor { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(5)]
        public string No { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string Type { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime Date { get; set; }

        [Key]
        [Column(Order = 6)]
        public TimeSpan Time_start { get; set; }

        [Key]
        [Column(Order = 7)]
        public TimeSpan Time_end { get; set; }
    }
}
