using LokalestyringUWP.Models;
using LokalestyringUWP.Service;
using LokalestyringUWP.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Handler
{
    class RoomHandler
    {
        public BookRoomsVM RoomReference { get; set; }
        public UserBookingsVM BookingReference { get; set; }
        public RoomHandler(BookRoomsVM r)
        {
            RoomReference = r;
        }
        public static void SaveRoomsAsync(Room obj)
        {
            PersistancyService.SaveRoomAsJsonAsync(obj);
        }

        public void FilterSearchMethod()
        {
            //add data

            //CheckBuilding();
            CheckRoomtype();
        }

        //public void CheckBuilding()
        //{
        //    foreach (var item in RoomReference.RoomList.ToList())
        //    {
        //        if (RoomReference.SelectedBuildingFilter == "Alle")
        //        {
        //            // Do nothing
        //        }
        //        else if (item.Building1 != RoomReference.SelectedBuildingFilter)
        //        {
        //            RoomReference.RoomList.Remove(item);
        //        }
        //    }
        //}

        public void CheckRoomtype()
        {

            foreach (var item in RoomReference.RoomList.ToList())
            {
                if (RoomReference.SelectedRoomtypeFilter == "Alle")
                {
                    // Do nothing
                }
                else if (item.RoomType != RoomReference.SelectedRoomtypeFilter)
                {
                    RoomReference.RoomList.Remove(item);
                }
            }
        }
        public void CheckDate()
        {

        }

        public void CheckTime()
        {

        }
    }
}
