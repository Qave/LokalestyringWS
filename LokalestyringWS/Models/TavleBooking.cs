namespace LokalestyringWS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TavleBooking")]
    public partial class TavleBooking
    {
        [Key]
        public int Tavle_Id { get; set; }

        public int Booking_Id { get; set; }

        public TimeSpan Time_start { get; set; }

        public TimeSpan Time_end { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
