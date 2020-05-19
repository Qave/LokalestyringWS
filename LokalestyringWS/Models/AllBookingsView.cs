namespace LokalestyringWS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AllBookingsView")]
    public partial class AllBookingsView
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Booking_Id { get; set; }

        [StringLength(58)]
        public string RoomName { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime Date { get; set; }

        [Key]
        [Column(Order = 2)]
        public TimeSpan BookingStart { get; set; }

        [Key]
        [Column(Order = 3)]
        public TimeSpan BookingEnd { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Room_Id { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Floor { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(5)]
        public string No { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(1)]
        public string Building_Letter { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(50)]
        public string Type { get; set; }

        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int User_Id { get; set; }
    }
}
