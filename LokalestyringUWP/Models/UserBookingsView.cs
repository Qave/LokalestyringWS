using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models
{
    public class UserBookingsView
    {
        public string RoomName { get; set; }

        public string City { get; set; }

        public string Building_Letter { get; set; }

        public int Floor { get; set; }

        public string No { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time_start { get; set; }

        public TimeSpan Time_end { get; set; }

        public int User_Id { get; set; }

        public string User_Name { get; set; }

        public string User_Email { get; set; }

        public bool Teacher { get; set; }
    }
}
