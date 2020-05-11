using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models
{
    class Booking
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

        public int? Tavle_Id { get; set; }

        public virtual TavleBooking TavleBooking { get; set; }

        public override string ToString()
        {
            return $"{Booking_Id}, {User_Id}, {Room_Id}, {Date}, {Time_start}, {Time_end}, {Tavle_Id}";
        }

        public virtual ICollection<TavleBooking> TavleBookings { get; set; }
    }
}
