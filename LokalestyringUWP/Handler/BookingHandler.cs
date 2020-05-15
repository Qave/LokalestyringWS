using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LokalestyringUWP.Models;
using LokalestyringUWP.Service;

namespace LokalestyringUWP.Handler
{
    public static class BookingHandler
    {
        public static DateTime SelectedDate { get; set; }
        public static TimeSpan SelectedTimeStart { get; set; }
        public static TimeSpan SelectedTimeEnd { get; set; }
        public static int RoomId { get; set; }
        public static ICollection<TavleBooking> TavleBooking { get; set; }
        public static int BookingId { get; set; }

        public static Room SelectedRoom { get; set; }


        


    }
}
