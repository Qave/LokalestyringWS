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

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Booking_Id { get; set; }

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
        public int Tavle_Id { get; set; }

        [Key]
        [Column(Order = 5)]
        public TimeSpan TavleStart { get; set; }

        [Key]
        [Column(Order = 6)]
        public TimeSpan TavleEnd { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Room_Id { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Floor { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(5)]
        public string No { get; set; }

        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Loc_Id { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(50)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(50)]
        public string City { get; set; }

        [Key]
        [Column(Order = 13)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Building_Id { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(1)]
        public string Building_Letter { get; set; }

        [StringLength(80)]
        public string Title { get; set; }

        [Key]
        [Column(Order = 15)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Type_Id { get; set; }

        [Key]
        [Column(Order = 16)]
        [StringLength(50)]
        public string Type { get; set; }

        [Key]
        [Column(Order = 17)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Booking_Limit { get; set; }

        [Key]
        [Column(Order = 18)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int User_Id { get; set; }

        [Key]
        [Column(Order = 19)]
        [StringLength(50)]
        public string User_Name { get; set; }

        [Key]
        [Column(Order = 20)]
        [StringLength(100)]
        public string User_Email { get; set; }

        [Key]
        [Column(Order = 21)]
        public bool Teacher { get; set; }
    }
}
