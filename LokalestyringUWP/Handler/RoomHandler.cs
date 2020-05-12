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
            foreach (var item in Reference.RoomList.ToList())
            {
                if (item.Building1 != "A")
                {
                    Reference.RoomList.Remove(item);
                }
            }
        }
    }
}
