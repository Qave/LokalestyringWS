using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;
using LokalestyringUWP.Service;
using LokalestyringUWP.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using LokalestyringUWP.Annotations;
using LokalestyringUWP.View;
using Windows.UI.Xaml.Input;

namespace LokalestyringUWP.Handler
{
    public class RoomHandler
    {
        public static BookRoomsVM RoomReference { get; set; }
        public RoomHandler(BookRoomsVM r)
        {
            RoomReference = r;
        }

        #region FILTER LOGIC
        /// <summary>
        /// Filters rooms by location, building, roomtype, date and time. If the chosen time or date is not valid, a dialog message is shown, asking the user to pick a valid time or date.
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
                CheckUserDoubleBooking();
            }
        }
        /// <summary>
        /// Filters by selected building. If "Alle" is selected, it doesn't filter. If selected BuildingFilter matches with the building_Letter in RoomList, it is added to the tempList.
        /// RoomList is then cleared and the tempList items is added back to RoomList.
        /// </summary>
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

        /// <summary>
        /// Filters booked class rooms by grouping the room id's in the booking table. If the room id has a count of 2, it is then removed from RoomList. 
        /// If the count has 1 or 0, the Booking_Limit property is updated respectively. 
        /// </summary>
        public void CheckBookingLimit()
        {
            foreach (var item in RoomReference.RoomList)
            {
                item.Booking_Limit = 0;
            }
            if (RoomReference.SelectedRoomtypeFilter == "Klasselokale" || RoomReference.SelectedRoomtypeFilter == "Alle")
            {
                var query = (from b in BookingCatalogSingleton.Instance.Bookings
                             join r in RoomReference.RoomList on b.Room_Id equals r.Room_Id
                             where b.Date.Equals(RoomReference.Date.DateTime) && b.Time_end >= RoomReference.TimeStart && b.Time_start <= RoomReference.TimeEnd && r.Type == "Klasselokale"
                             group b by b.Room_Id into RoomGroup
                             select new
                             {
                                 LimitKey = RoomGroup.Key,
                                 Count = RoomGroup.Count()
                             }).ToList();
                if (BookingCatalogSingleton.Instance.Bookings.Count != 0)
                {
                    foreach (var klasseLokaler in query)
                    {
                        var query1 = (from r in RoomReference.RoomList
                                      where r.Room_Id.Equals(klasseLokaler.LimitKey)
                                      select r).ToList();
                        foreach (var variable in query1)
                        {
                            if (klasseLokaler.Count >= 2)
                            {
                                RoomReference.RoomList.Remove(variable);
                            }
                            if (klasseLokaler.Count == 1)
                            {
                                foreach (var item in RoomReference.RoomList)
                                {
                                    if (variable.Room_Id == item.Room_Id)
                                    {
                                        item.Booking_Limit = 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Filters by date and time. With LINQ the tables from the RoomsView table and the booking table are joined, so the date and time properties are accessible.
        /// The date and time properties are compared with the selected date and time properties. If the comparison is true (room is booked), it gets added to the query.
        /// Afterwards the items in the query is removed from the original roomlist in the view, meaning it has removed all booked rooms from the list. 
        /// </summary>
        public void CheckDateAndTime()
        {
            if (RoomReference.SelectedRoomtypeFilter != "Klasselokale")
            {

                var query = (from r in RoomReference.RoomList
                             join b in BookingCatalogSingleton.Instance.Bookings on r.Room_Id equals b.Room_Id into temp
                             from t in temp
                             where t.Date.Equals(RoomReference.Date.DateTime) && t.Time_end >= RoomReference.TimeStart && t.Time_start <= RoomReference.TimeEnd && r.Type != "Klasselokale"
                             select r).ToList();

                foreach (var item in query)
                {
                    RoomReference.RoomList.Remove(item);
                }
            }
        }

        /// <summary>
        /// Filters rooms that the user already have booked in selected time interval. You are only able to book one room at a specific time and date per user. 
        /// </summary>
        public void CheckUserDoubleBooking()
        {
            var query = (from r in RoomReference.RoomList
                         join q in BookingCatalogSingleton.Instance.Bookings on r.Room_Id equals q.Room_Id into bookedRooms
                         from qr in bookedRooms
                         where qr.User_Id == LoginHandler.SelectedUser.User_Id && qr.Date == RoomReference.Date && qr.Time_end >= RoomReference.TimeStart && qr.Time_start <= RoomReference.TimeEnd
                         select r).ToList();

            foreach (var item in query)
            {
                RoomReference.RoomList.Remove(item);
            }
        }

        #endregion

        /// <summary>
        ///  Adds all items from the singleton list to a new list called "ResettedList". Then filters by selected location. 
        /// </summary>
        public static void RestoreList()
        {
            var query = (from q in RoomsViewCatalogSingleton.Instance.RoomsView
                         where RoomReference.selectedLocation == q.City
                         select q).ToList();
            RoomReference.RoomList.Clear();
            foreach (var item in query)
            {
                RoomReference.RoomList.Add(item);
            }
        }

        /// <summary>
        /// When called, it sends the user back to the previous page. It also deselects the selected location, so you're able to pick a new or the same location again.
        /// </summary>
        public static void GoBackMethod()
        {
            ((Frame)Window.Current.Content).Navigate(typeof(PageLocations));
            LocationsVM.SelectedLocation = null;
        }

        /// <summary>
        /// Creates new booking object and sends an email with confirmation of the booking. 
        /// If the user already has booked a room with the same date and time interval, a pop-up will apear telling the user he needs to switch date or time.
        /// </summary>
        public async void CreateBooking()
        {
            bool variable = true;
            foreach (var item in BookingCatalogSingleton.Instance.Bookings)
            {
                if (item.User_Id == LoginHandler.SelectedUser.User_Id && item.Date == RoomReference.Date && item.Time_end >= RoomReference.TimeStart && item.Time_start <= RoomReference.TimeEnd)
                {
                    DialogHandler.Dialog("Du har allerede booket et lokale på denne dato i samme tidsinterval. Vælg venligst et nyt tidspunkt.", "Overlappende booking");
                    variable = false;
                    break;
                }
            }
            if (variable)
            {
                // I don't know why, but we need this reference to get the RoomName property in the RoomsView model.
                RoomsView selectedRoomsViewRef = RoomReference.SelectedRoomsView;
                var result = await DialogHandler.GenericYesNoDialog("Er du sikker på du vil booke dette lokale?", "Book lokale?", "Ja, tak", "Nej, tak");
                Booking booking = new Booking()
                {
                    Date = RoomReference.Date.Date,
                    Room_Id = RoomReference.SelectedRoomsView.Room_Id,
                    Time_start = new TimeSpan(RoomReference.TimeStart.Hours, RoomReference.TimeStart.Minutes, 0),
                    Time_end = new TimeSpan(RoomReference.TimeEnd.Hours, RoomReference.TimeEnd.Minutes, 0),
                    User_Id = LoginHandler.SelectedUser.User_Id
                };

                if (result)
                {
                    BookingCatalogSingleton.Instance.Bookings.Add(booking);
                    FilterSearchMethod();
                    await MailService.MailSender(LoginHandler.SelectedUser.User_Email, "Kvittering på booking", $"Du har booket {selectedRoomsViewRef.RoomName} " +
                        $"d. {RoomReference.Date.ToString("dd/MM/yyyy")} " +
                        $"mellem {new DateTime(RoomReference.TimeStart.Ticks).ToString("HH:mm")} og {new DateTime(RoomReference.TimeEnd.Ticks).ToString("HH:mm")}.", true);
                    RoomReference.SelectedRoomsView = null;
                    await PersistancyService.SaveInsertAsJsonAsync(booking, "Bookings");
                }
            }
        }
    }
}
