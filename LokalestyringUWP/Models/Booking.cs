using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models
{
    public class Booking
    {
        public Booking()
        {
        }

        public int Booking_Id { get; set; }

        public int User_Id { get; set; }

        public int Room_Id { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time_start { get; set; }

        public TimeSpan Time_end { get; set; }

        public virtual ICollection<TavleBooking> TavleBookings { get; set; }
    }
}
