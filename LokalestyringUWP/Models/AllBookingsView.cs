using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models
{
    public class AllBookingsView
    {

        public int Booking_Id { get; set; }

        public string RoomName { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan BookingStart { get; set; }

        public TimeSpan BookingEnd { get; set; }

        public int Room_Id { get; set; }

        public int Floor { get; set; }

        public string No { get; set; }

        public string Name { get; set; }

        public string Building_Letter { get; set; }

        public string Type { get; set; }

        public int User_Id { get; set; }
    }
}
