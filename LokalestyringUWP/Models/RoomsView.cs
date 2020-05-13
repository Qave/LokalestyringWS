using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models
{
    class RoomsView
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
    }
}
