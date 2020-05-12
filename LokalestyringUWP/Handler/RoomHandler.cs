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
            var filteredList = from fl in Reference.RoomList
                               where fl.RoomType.Contains("Lille") && fl.Building1.Contains("B")
                               select fl;

            Reference.RoomList.Clear();
            foreach (var item in filteredList)
            {
                Reference.RoomList.Add(item);
            }
        }
    }
}
