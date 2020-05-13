using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models
{
    public class TavleBooking
    {
        public TavleBooking()
        {
        }

        public int Tavle_Id { get; set; }

        public int Booking_Id { get; set; }

        public TimeSpan Time_start { get; set; }

        public TimeSpan Time_end { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        public virtual Booking Booking { get; set; }

        public override string ToString()
        {
            return $"{Tavle_Id}, {Booking_Id}, {Time_start}, {Time_end}";
        }
    }
}
