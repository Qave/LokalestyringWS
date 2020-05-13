using LokalestyringUWP.Models;
using LokalestyringUWP.Service;
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
        public ViewModel.ViewModel Reference { get; set; }
        public RoomHandler(ViewModel.ViewModel p)
        {
            Reference = p;
        }
        public static void SaveRoomsAsync(Room obj)
        {
            PersistancyService.SaveRoomAsJsonAsync(obj);
        }

        public void FilterSearchMethod()
        {
            //add data
            CheckBuilding();
            CheckRoomtype();
        }

        public void CheckBuilding()
        {
            foreach (var item in Reference.RoomList.ToList())
            {
                if (Reference.SelectedBuildingFilter == "Alle")
                {
                    // Do nothing
                }
                else if (item.Building1 != Reference.SelectedBuildingFilter)
                {
                    Reference.RoomList.Remove(item);
                }
            }
        }

        public void CheckRoomtype()
        {

            foreach (var item in Reference.RoomList.ToList())
            {
                if (Reference.SelectedRoomtypeFilter == "Alle")
                {
                    // Do nothing
                }
                else if (item.RoomType != Reference.SelectedRoomtypeFilter)
                {
                    Reference.RoomList.Remove(item);
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
