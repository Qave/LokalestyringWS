using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models
{
    public class AllBookingsView
    {
        public string RoomName { get; set; }

        public int Booking_Id { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? BookingStart { get; set; }

        public TimeSpan? BookingEnd { get; set; }

        public int? Tavle_Id { get; set; }

        public TimeSpan? TavleStart { get; set; }

        public TimeSpan? TavleEnd { get; set; }

        public int Room_Id { get; set; }

        public int Floor { get; set; }

        public string No { get; set; }

        public int Loc_Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public int Building_Id { get; set; }

        public string Building_Letter { get; set; }

        public string Title { get; set; }

        public int Type_Id { get; set; }

        public string Type { get; set; }

        public int Booking_Limit { get; set; }

        public int User_Id { get; set; }

        public string User_Name { get; set; }

        public string User_Email { get; set; }

        public bool Teacher { get; set; }
    }
}
