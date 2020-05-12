using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models
{
    class Room
    {
        public Room(int room_Id, int floor, string no, int type_Id, int building_Id, int loc_Id, string building, string location, string roomType)
        {
            Room_Id = room_Id;
            Floor = floor;
            No = no;
            Type_Id = type_Id;
            Building_Id = building_Id;
            Loc_Id = loc_Id;
            Building1 = building;
            Location1 = location;
            RoomType = roomType;
        }

        public int Room_Id { get; set; }

        public int Floor { get; set; }

        public string FloorString 
        { 
            get { return $"Etage: {Floor}"; }
        }

        public string BuildingString 
        { 
            get { return $"Bygning: {Building1}"; } 
        }
        public string No { get; set; }

        public int Type_Id { get; set; }

        public int Building_Id { get; set; }

        public int Loc_Id { get; set; }

        public string Building1 { get; }

        public string Location1 { get; }

        public string RoomType { get; }

        public string RoomIdString
        {
            get { return $"{Location1}-{Building1}.{No}"; }
        }

        public virtual Building Building { get; set; }

        public virtual Location Location { get; set; }

        public virtual Roomtype Roomtype { get; set; }

        //public override string ToString()
        //{
        //    return $"{Room_Id}, {Floor}, {No}, {Type_Id}, {Building_Id}, {Loc_Id}";
        //}
    }
}
