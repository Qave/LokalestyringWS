using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models
{
    public class Roomtype
    {
        public Roomtype()
        {
        }

        public int Type_Id { get; set; }

        public string Type { get; set; }

        public int Booking_Limit { get; set; }

        public override string ToString()
        {
            return $"{Type_Id}, {Type}, {Booking_Limit}";
        }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
