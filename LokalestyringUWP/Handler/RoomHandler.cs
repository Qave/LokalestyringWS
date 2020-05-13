using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;
using LokalestyringUWP.Service;
using LokalestyringUWP.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace LokalestyringUWP.Handler
{
    class RoomHandler
    {
        public BookRoomsVM RoomReference { get; set; }
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
            RestoreList();
            CheckBuilding();
            CheckRoomtype();
        }

        public void CheckBuilding()
        {
            var tempList = (from tl in RoomReference.RoomList
                            where tl.Building_Letter == RoomReference.SelectedBuildingFilter
                            select tl).ToList();

            RoomReference.RoomList.Clear();
            foreach (var item in tempList)
            {
                RoomReference.RoomList.Add(item);
            }

        }

        public void RestoreList()
        {
            //ResettedList er en list der bruges til at reset listen til de objekter der findes i databasen. 
            if (RoomReference.ResettedList.Count == 0)
            {
                foreach (var item in RoomReference.RoomList)
                {
                    RoomReference.ResettedList.Add(item);
                }
            }
            RoomReference.RoomList.Clear();
            foreach (var item in RoomReference.ResettedList)
            {
                RoomReference.RoomList.Add(item);
            }
        }
        public void CheckRoomtype()
        {
            if (RoomReference.SelectedRoomtypeFilter == "Alle")
            {
                // Do nothing
            }
            else
            {
                var tempList = (from tl in RoomReference.RoomList
                                where tl.Type == RoomReference.SelectedRoomtypeFilter
                                select tl).ToList();

                RoomReference.RoomList.Clear();
                foreach (var item in tempList)
                {
                    RoomReference.RoomList.Add(item);
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
