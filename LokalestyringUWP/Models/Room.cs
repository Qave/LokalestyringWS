using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models
{
    class Room
    {
        public Room(Building b)
        {
            Building = b;
        }

        public int Room_Id { get; set; }

        public int Floor { get; set; }

        public string FloorString 
        { 
            get { return $"Etage: {Floor}"; }
        }

        // LORTET VIRKER IKKE; NULLREFEXCEPTION ??? HVORDAN FÅR MAN PROPERTIES FRA ANDRE TABELLER TIL AT VISES I ROOM TABELLEN UD FRA ID??
        //public string BuildingString
        //{
        //    get { return $"Bygning: {Building.Building_Letter}"; }
        //}
        public string No { get; set; }

        public int Type_Id { get; set; }

        public int Building_Id { get; set; }

        public int Loc_Id { get; set; }

        public string RoomType { get; }

        //public string RoomIdString
        //{
        //    get { return $"{Location.City}-{Building.Building_Letter}.{No}"; }
        //}

        public virtual Building Building { get; set; }

        public virtual Location Location { get; set; }

        public virtual Roomtype Roomtype { get; set; }
    }
}
