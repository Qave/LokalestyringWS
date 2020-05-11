using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models
{
    class Location
    {
        public Location()
        {
        }

        public int Loc_Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public override string ToString()
        {
            return $"{Loc_Id}, {Name}, {City}";
        }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
