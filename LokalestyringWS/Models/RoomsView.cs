namespace LokalestyringWS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoomsView")]
    public partial class RoomsView
    {
        [StringLength(58)]
        public string RoomName { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Room_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Floor { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(5)]
        public string No { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Type { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Booking_Limit { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string City { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(1)]
        public string Building_Letter { get; set; }
    }
}
