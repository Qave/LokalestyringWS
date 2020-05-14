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
        public static DateTime Date { get; set; }
        public static TimeSpan TimeStart { get; set; }
        public static TimeSpan TimeEnd { get; set; }
        public static int UserId { get; set; }
        public static int RoomId { get; set; }
        public static ICollection<TavleBooking> TavleBooking { get; set; }
        public static int BookingId { get; set; }


        public static void CreateBooking()
        {
            PersistancyService.SaveBookingAsJsonAsync(new Booking(){Date = Date,Booking_Id = BookingId,Room_Id = RoomId,TavleBookings = TavleBooking, Time_end = TimeEnd, Time_start = TimeStart, User_Id = UserId});
        }


    }
}
