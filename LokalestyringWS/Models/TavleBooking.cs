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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TavleBooking()
        {
            Bookings = new HashSet<Booking>();
        }

        [Key]
        public int Tavle_Id { get; set; }

        public int Booking_Id { get; set; }

        public TimeSpan Time_start { get; set; }

        public TimeSpan Time_end { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }

        public virtual Booking Booking { get; set; }

        public override string ToString()
        {
            return $"{Tavle_Id}, {Booking_Id}, {Time_start}, {Time_end}";
        }
    }
}
