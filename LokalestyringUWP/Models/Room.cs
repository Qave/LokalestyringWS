using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models
{
    public class Room
    {
        public Room()
        {
        }

        public int Room_Id { get; set; }

        public int Floor { get; set; }

        public string No { get; set; }

        public int Type_Id { get; set; }

        public int Building_Id { get; set; }

        public int Loc_Id { get; set; }

        public string RoomType { get; }


        public virtual Building Building { get; set; }

        public virtual Location Location { get; set; }

        public virtual Roomtype Roomtype { get; set; }
    }
}
