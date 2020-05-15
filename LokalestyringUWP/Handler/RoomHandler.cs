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
        /// <summary>
        /// Filters rooms by building, roomtype and date and time. If the chosen time is not valid, a dialog message is shown, asking the user to pick a valid time.
        /// 
        /// </summary>
        public void FilterSearchMethod()
        {
            DateTime currentDate = DateTime.Now;
            if (RoomReference.TimeStart >= RoomReference.TimeEnd)
            {
                DialogHandler.Dialog("Vælg venligst en gyldig start- og sluttid.", "Ugyldigt tidspunkt");
            }
            else if (RoomReference.Date.DateTime < currentDate.Date)
            {
                DialogHandler.Dialog("Vælg venligst en gyldig dato fra denne måned eller frem", "Ugyldig dato");
            }
            else
            {
                RestoreList();
                CheckBuilding();
                CheckRoomtype();
                CheckBookingLimit();
                CheckDateAndTime();
            }
        }
        /// <summary>
        /// Filters by selected building. If "Alle" is selected, it doesn't filter. If selected BuildingFilter matches with the building_Letter in RoomList, it is added to the tempList.
        /// RoomList is then cleared and the tempList items is added back to RoomList.
        /// </summary>
        /// 
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

        /// <summary>
        /// Filters by selected roomtype. If "Alle" is selected, it doesn't filter. If selected RoomtypeFilter matches with the roomtype in RoomList, it is added to the tempList.
        /// RoomList is then cleared and the tempList items is added back to RoomList.
        /// </summary>
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
        public void CheckBookingLimit()
        {
            if (RoomReference.SelectedRoomtypeFilter == "Klasselokale" || RoomReference.SelectedRoomtypeFilter == "Alle")
            {
                var query = (from b in BookingReference.Bookings
                             join r in RoomReference.RoomList on b.Room_Id equals r.Room_Id
                             where b.Date.Equals(RoomReference.Date.DateTime) && b.Time_end >= RoomReference.TimeStart && b.Time_start <= RoomReference.TimeEnd && r.Type == "Klasselokale"
                             group b by b.Room_Id into RoomGroup
                             select new
                             {
                                 LimitKey = RoomGroup.Key,
                                 Count = RoomGroup.Count()
                             }).ToList();

                foreach (var klasseLokaler in query)
                {
                    if (klasseLokaler.Count >= 2)
                    {
                        var query1 = (from r in RoomReference.RoomList
                                      where r.Room_Id.Equals(klasseLokaler.LimitKey)
                                      select r).ToList();
                        foreach (var variable in query1)
                        {
                            RoomReference.RoomList.Remove(variable);
                        }
                    }
                }

            }
        }
        /// <summary>
        /// Filters by date and time. With LINQ we join the tables from the RoomsView table and the booking table, so we're able to use the date and time properties.
        /// The date and time properties are compared with the selected date and time properties. If the comparison is true (meaning the room is booked), it gets added to the query.
        /// Afterwards the items in the query is removed from the original roomlist in the view, meaning it has removed all booked rooms from the list. 
        /// </summary>
        public void CheckDateAndTime()
        {
            if (RoomReference.SelectedRoomtypeFilter != "Klasselokale")
            {

                var query = (from r in RoomReference.RoomList
                             join b in BookingReference.Bookings on r.Room_Id equals b.Room_Id into temp
                             from t in temp
                             where t.Date.Equals(RoomReference.Date.DateTime) && t.Time_end >= RoomReference.TimeStart && t.Time_start <= RoomReference.TimeEnd && r.Type != "Klasselokale"
                             select r).ToList();

                foreach (var item in query)
                {
                    RoomReference.RoomList.Remove(item);
                }
            }
        }

        #endregion

        /// <summary>
        /// Resets the list, so every time you want to change filter, you can do it on the fly without having to restart the program. 
        /// Gets called when a location is selected or the filter button is clicked. 
        /// </summary>
        public static void RestoreList()
        {
            if (RoomReference.ResettedList.Count == 0)
            {
                foreach (var item in RoomsViewCatalogSingleton.Instance.RoomsView)
                {
                    RoomReference.ResettedList.Add(item); //Adds all items from the singleton into a new resetted list, that we can use to filter with.
                }

                //Filters the list by selected location.
                var query = (from q in RoomReference.ResettedList
                             where RoomReference.selectedLocation == q.City
                             select q).ToList();

                //The resetted list is cleared and is filled with the results from the LINQ statement.
                RoomReference.ResettedList.Clear();
                foreach (var item in query)
                {
                    RoomReference.ResettedList.Add(item);
                }
            }

            //The list that gets shown is cleared, and the filtered results gets added to it, so it gets updated in the view.
            RoomReference.RoomList.Clear();
            foreach (var item in RoomReference.ResettedList)
            {
                RoomReference.RoomList.Add(item);
            }
        }

        /// <summary>
        /// When called, it sends the user back to the previous page. It also deselects the selected location, so you're able to pick a new or the same location again.
        /// </summary>
        public static void GoBackMethod()
        {
            ((Frame)Window.Current.Content).GoBack();
            LocationsVM.SelectedLocation = null;
        }

    }
}
