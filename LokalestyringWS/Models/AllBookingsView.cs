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
        [StringLength(58)]
        public string RoomName { get; set; }

        public int Booking_Id { get; set; }

        public DateTime? Date { get; set; }

        public TimeSpan? BookingStart { get; set; }

        public TimeSpan? BookingEnd { get; set; }

        public int? Tavle_Id { get; set; }

        public TimeSpan? TavleStart { get; set; }

        public TimeSpan? TavleEnd { get; set; }

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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Loc_Id { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string City { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Building_Id { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(1)]
        public string Building_Letter { get; set; }

        [StringLength(80)]
        public string Title { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Type_Id { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(50)]
        public string Type { get; set; }

        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Booking_Limit { get; set; }

        [Key]
        [Column(Order = 11)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int User_Id { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(50)]
        public string User_Name { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(100)]
        public string User_Email { get; set; }

        [Key]
        [Column(Order = 14)]
        public bool Teacher { get; set; }
    }
}
