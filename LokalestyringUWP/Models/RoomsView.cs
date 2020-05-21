using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models
{
    public class RoomsView
    {
        public int MaxBookings 
        {
            get
            {
                if (Type == "Klasselokale")
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }

        }

        public string RoomName { get; set; }

        public int Room_Id { get; set; }

        public int Floor { get; set; }

        public string No { get; set; }

        public string Type { get; set; }

        public int Booking_Limit { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string Building_Letter { get; set; }

        public string Booking_LimitString 
        {
            get { return $"{Booking_Limit}/{MaxBookings}"; }  
        }

        public string FloorString 
        { 
            get { return $"{Floor}"; } 
        }

        public string BuildingString 
        { 
            get { return $"{Building_Letter}"; } 
        }

    }
}
