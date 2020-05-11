namespace LokalestyringWS.Models
{
    using Antlr.Runtime;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Booking")]
    public partial class Booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Booking()
        {
            TavleBookings = new HashSet<TavleBooking>();
        }

        [Key]
        public int Booking_Id { get; set; }

        public int User_Id { get; set; }

        public int Room_Id { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time_start { get; set; }

        public TimeSpan Time_end { get; set; }

        public int? Tavle_Id { get; set; }

        public virtual TavleBooking TavleBooking { get; set; }

        public override string ToString()
        {
            return $"{Booking_Id}, {User_Id}, {Room_Id}, {Date}, {Time_start}, {Time_end}, {Tavle_Id}"; 
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TavleBooking> TavleBookings { get; set; }
    }
}
