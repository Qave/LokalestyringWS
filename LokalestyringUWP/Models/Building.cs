using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models
{
    class Building
    {
        public Building()
        {
        }

        public int Building_Id { get; set; }

        public string Building_Letter { get; set; }

        public string Title { get; set; }

        public override string ToString()
        {
            return $"{Building_Id}, {Building_Letter}, {Title}";
        }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
