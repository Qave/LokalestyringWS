﻿using LokalestyringUWP.Models;
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
    public class RoomHandler
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

        #region FILTER LOGIC

        //Filters rooms by building, roomtype and date and time.
        public void FilterSearchMethod()
        {
            if (RoomReference.TimeStart >= RoomReference.TimeEnd || RoomReference.TimeStart == null || RoomReference.TimeEnd == null) //If the chosen time is not valid, a dialog message is shown, asking the user to pick a valid time.
            {
                DialogHandler.Dialog("Vælg venligst en gyldig start- og sluttid.", "Ugyldigt tidspunkt");
            }
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

            var query = (from r in RoomReference.RoomList
                         join b in BookingReference.Bookings on r.Room_Id equals b.Room_Id into temp
                         from t in temp
                         where t.Date.Equals(RoomReference.Date.DateTime) && t.Time_end >= RoomReference.TimeStart && t.Time_start <= RoomReference.TimeEnd
                         select r).ToList();
                                                                                 
            foreach (var item in query)
            {
                RoomReference.RoomList.Remove(item);
            }
            //foreach (var item in query)
            //{
            //    RoomReference.RoomList.Add(item);
            //}

        }

        #endregion

        //This method is used to "reset" the list, so every time you want to change filter, you can do it on the fly without having to restart the program. 
        public static void RestoreList()
        {
            if (RoomReference.ResettedList.Count == 0)
            {
                foreach (var item in RoomsViewCatalogSingleton.Instance.RoomsView)
                {
                    RoomReference.ResettedList.Add(item); //Adds all items from the singleton into a new resetted list, that we can use to filter with.
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

        public static void GoBackMethod()
        {
            ((Frame)Window.Current.Content).GoBack();
            LocationsVM.SelectedLocation = null;
        }

    }
}
