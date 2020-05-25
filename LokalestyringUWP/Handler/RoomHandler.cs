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
using Windows.UI.Xaml.Data;

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
        DateTime currentDate = DateTime.Now.Date;

        /// <summary>
        /// Returns true if a teacher has booked 3 rooms or more. 
        /// </summary>
        public bool ThreeRoomsBookingLimit()
        {
            var query = (from b in BookingCatalogSingleton.Instance.Bookings
                         where b.User_Id == LoginHandler.CurrentUserId &&
                               b.Date.Date == RoomReference.Date.Date
                               && b.Time_end >= RoomReference.TimeStart && b.Time_start <= RoomReference.TimeEnd
                         group b by b.User_Id into RoomGroup
                         select new
                         {
                             LimitKey = RoomGroup.Key,
                             Count = RoomGroup.Count()
                         }).ToList();

            foreach (var item in query)
            {
                if (item.Count >= 3)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// First compares times to see if a valid time has been selected. Afterwards it checks if a valid date has been selected. If both are valid, it returns true. 
        /// </summary>
        public bool CompareDatesAndTime()
        {
            if (RoomReference.TimeStart >= RoomReference.TimeEnd || RoomReference.TimeStart == TimeSpan.Zero || RoomReference.TimeEnd == TimeSpan.Zero)
            {
                DialogHandler.Dialog("Vælg venligst en gyldig start- og sluttid. Starttid eller sluttid kan ikke være 00.", "Ugyldigt tidspunkt");
                return false;
            }
            else if (RoomReference.Date.DateTime < currentDate)
            {
                DialogHandler.Dialog("Vælg venligst en gyldig dato fra denne måned eller frem", "Ugyldig dato");
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// A methods that first reloads the booking list with the database values and then calls all other filter methods. 
        /// </summary>
        public async void FilterSearchMethodAsync()
        {
            if (CompareDatesAndTime())
            {
                BookingCatalogSingleton.Instance.Bookings.Clear();
                await BookingCatalogSingleton.Instance.LoadbookingsAsync();
                RestoreList();
                CheckIfTeacher();
                CheckBookingLimit();
                CheckBuilding();
                CheckRoomtype();
                CheckDateAndTimeForSingleRooms();
                CheckUserDoubleBooking();
                RoomReference.Refresh();
            }
        }

        /// <summary>
        ///  Adds all items from the singleton list to a new list called "ResettedList". Then filters by selected location. 
        /// </summary>
        public void RestoreList()
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
        /// Filters by selected building. If "Alle" is selected, it doesn't filter.
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
        /// Filters by selected roomtype. If "Alle" is selected, it doesn't filter.
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
                                where tl.Type != RoomReference.SelectedRoomtypeFilter
                                select tl).ToList();

                foreach (var item in tempList)
                {
                    RoomReference.RoomList.Remove(item);
                }
            }
        }


        /// <summary>
        /// Filters booked class rooms by grouping the room id's in the booking table. If the room id has a count of 2, it is then removed from RoomList. 
        /// If the count has 1 or 0, the Booking_Limit property is updated respectively. 
        /// </summary>
        public void CheckBookingLimit()
        {
            foreach (var item in RoomReference.RoomList.ToList())
            {
                item.Booking_Limit = 0;
            }

            if (RoomReference.SelectedRoomtypeFilter == "Klasselokale" || RoomReference.SelectedRoomtypeFilter == "Alle")
            {
                var query = (from b in BookingCatalogSingleton.Instance.Bookings
                             where b.Date == RoomReference.Date.DateTime && b.Time_end >= RoomReference.TimeStart &&
                             b.Time_start <= RoomReference.TimeEnd
                             group b by b.Room_Id into RoomGroup
                             select new
                             {
                                 LimitKey = RoomGroup.Key,
                                 Count = RoomGroup.Count(),
                             }).ToList();

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
                            foreach (var item in RoomReference.RoomList.ToList())
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
        /// <summary>
        /// Filters by date and time rooms that are not class rooms.
        /// </summary>
        public void CheckDateAndTimeForSingleRooms()
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
        /// Removes rooms that are booked by teachers. 
        /// </summary>
        public void CheckIfTeacher()
        {
            var query = (from r in RoomReference.RoomList
                         join b in BookingCatalogSingleton.Instance.Bookings on r.Room_Id equals b.Room_Id
                         join u in UserCatalogSingleton.Instance.Users on b.User_Id equals u.User_Id
                         where u.Teacher && b.Date.Equals(RoomReference.Date.DateTime) && b.Time_end >= RoomReference.TimeStart && b.Time_start <= RoomReference.TimeEnd
                         select r).ToList();

            foreach (var item in query)
            {
                RoomReference.RoomList.Remove(item);
            }
        }
        /// <summary>
        /// Filters rooms that the user already have booked in selected time interval. You are only able to book one room at a specific time and date per student. 
        /// </summary>
        public void CheckUserDoubleBooking()
        {
            var query = (from r in RoomReference.RoomList
                         join q in BookingCatalogSingleton.Instance.Bookings on r.Room_Id equals q.Room_Id into bookedRooms
                         from qr in bookedRooms
                         where qr.User_Id == LoginHandler.SelectedUser.User_Id && qr.Date == RoomReference.Date && qr.Time_end >= RoomReference.TimeStart
                         && qr.Time_start <= RoomReference.TimeEnd
                         select r).ToList();

            foreach (var item in query)
            {
                RoomReference.RoomList.Remove(item);
            }
        }

        #endregion

        /// <summary>
        /// Check if room is selected.
        /// </summary>
        public bool RoomIsSelectedCheck()
        {
            return (RoomReference.SelectedRoomsView != null);
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
        /// Creates new booking object in the database and sends an email with confirmation of the booking, if the conditions are met. 
        /// </summary>
        public async void CreateBookingAsync()
        {
            bool bookedAlready = true;
            foreach (var item in BookingCatalogSingleton.Instance.Bookings)
            {
                if (item.User_Id == LoginHandler.SelectedUser.User_Id && item.Date ==
                    RoomReference.Date && item.Time_end >= RoomReference.TimeStart && item.Time_start <= RoomReference.TimeEnd && LoginHandler.SelectedUser.Teacher == false)
                {
                    DialogHandler.Dialog("Du har allerede booket et lokale på denne dato i samme tidsinterval. Vælg venligst et nyt tidspunkt.", "Overlappende booking");
                    bookedAlready = false;
                    break;
                }
            }

            if (LoginHandler.SelectedUser.Teacher)
            {
                if (ThreeRoomsBookingLimit() == false)
                {
                    bookedAlready = false;
                    DialogHandler.Dialog("Du kan ikke have mere end tre bookinger af gangen, hvis du vil booke dette rum må du slette en anden booking", "Kun tre bookinger");
                }
            }
            if (bookedAlready)
            {
                // I don't know why, but we need this reference to get the RoomName property in the RoomsView model.
                RoomsView selectedRoomsViewRef = RoomReference.SelectedRoomsView;
                var result = await DialogHandler.GenericYesNoDialog("Er du sikker på du vil booke dette lokale?", "Book lokale?", "Ja, tak", "Nej, tak");
                if (CompareDatesAndTime() && result)
                {
                    Booking booking = new Booking()
                    {
                        Date = RoomReference.Date.Date,
                        Room_Id = selectedRoomsViewRef.Room_Id,
                        Time_start = new TimeSpan(RoomReference.TimeStart.Hours, RoomReference.TimeStart.Minutes, 0),
                        Time_end = new TimeSpan(RoomReference.TimeEnd.Hours, RoomReference.TimeEnd.Minutes, 0),
                        User_Id = LoginHandler.SelectedUser.User_Id
                    };

                    await PersistancyService.SaveInsertAsJsonAsync(booking, "Bookings");
                    BookingCatalogSingleton.Instance.Bookings.Clear();
                    await BookingCatalogSingleton.Instance.LoadbookingsAsync();
                    FilterSearchMethodAsync();
                    // Does not need to be awaited, since it doesn't disrupt the flow of the program. 
                    MailService.MailSenderAsync(LoginHandler.SelectedUser.User_Email, "Kvittering på booking", $"Du har booket {selectedRoomsViewRef.RoomName} " +
                        $"d. {RoomReference.Date.ToString("dd/MM/yyyy")} " +
                        $"mellem {new DateTime(RoomReference.TimeStart.Ticks).ToString("HH:mm")} og {new DateTime(RoomReference.TimeEnd.Ticks).ToString("HH:mm")}.", true);
                    RoomReference.SelectedRoomsView = null;
                }
            }
        }
    }
}
