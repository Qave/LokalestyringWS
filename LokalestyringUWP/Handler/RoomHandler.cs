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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace LokalestyringUWP.Handler
{
    class RoomHandler
    {
        public static BookRoomsVM RoomReference { get; set; }
        public BookingCatalogSingleton BookingReference { get; set; }
        public RoomHandler(BookRoomsVM r)
        {
            BookingReference = new BookingCatalogSingleton();
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
            CheckDateAndTime();
        }

        public void CheckBuilding()
        {
            if (RoomReference.SelectedBuildingFilter == "Alle")
            {
                // Do nothing
            }
            else
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

        }

        public static void RestoreList()
        {
            //ResettedList er en list der bruges til at reset listen til de objekter der findes i databasen. 
            if (RoomReference.ResettedList.Count == 0)
            {
                foreach (var item in RoomsViewCatalogSingleton.Instance.RoomsView)
                {
                    RoomReference.ResettedList.Add(item);
                }
                var query = (from q in RoomReference.ResettedList
                            where RoomReference.selectedLocation == q.City
                            select q).ToList();
                RoomReference.ResettedList.Clear();
                foreach (var item in query)
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

        //Check Room_ID match in Bookings with Room_Id in rooms - If ID match, check dates and time
        public void CheckDateAndTime()
        {
            List<Booking> newBookingList = BookingReference.Bookings.ToList();

            //var rooms = from r in RoomCatalogSingleton.Instance.Rooms
            //            join b in BookingReference.Bookings on r.Room_Id equals b.Room_Id
            //            where b.

        }

        public static void GoBackMethod()
        {
            ((Frame)Window.Current.Content).GoBack();
            LocationsVM.SelectedLocation = null;
        }

    }
}
