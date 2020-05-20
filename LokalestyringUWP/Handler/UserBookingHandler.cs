using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;
using LokalestyringUWP.Service;
using LokalestyringUWP.View;
using LokalestyringUWP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LokalestyringUWP.Handler
{
    public class UserBookingHandler 
    {
        public static UserBookingsVM Reference { get; set; }
        public UserBookingHandler(UserBookingsVM r)
        {
            Reference = r;
        }

        /// <summary>
        /// Async method that calls the async delete method from the persistancyService that deletes the selected booking from the database
        /// </summary>
        public static async Task CancelBookingMethodAsync()
        {
            // Checks if the user wants to delete the booking, or not
            var result = await DialogHandler.GenericYesNoDialog("Er du sikker på du vil Aflyse denne bookning?\nTilhørende tavlebookings vil også blive Aflyst.", "Aflys Bookning?", "Ja, Aflys booking", "Fortryd");
            // If user wants to delete the booking.
            if (result)
            {
                var query = (from q in BookingCatalogSingleton.Instance.Bookings
                            where q.Booking_Id == Reference.SelectedBooking.Booking_Id
                            select q).ToList();
                foreach (var item in query)
                {
                        BookingCatalogSingleton.Instance.Bookings.Remove(item);
                }

                // The async delete method from PersistancyService.
                PersistancyService.DeleteFromDatabaseAsync("Bookings", Reference.SelectedBooking.Booking_Id);

                // Deletes the selected object from the singleton observable collection
                Reference.AllUserBookingsFromSingleton.Remove(Reference.SelectedBooking);
                // Update the view
                Reference.ElementIsChosenVisibility = Visibility.Collapsed;
                Reference.NoElementsChosenVisibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Async method that calls the async delete method from the persistancyService that deletes the selected booking's Tavle booking from the database 
        /// </summary>
        public static async Task CancelTavleBookingMethodAsync()
        {
            // Checks if the user wants to delete the TavleBooking, or not
            var result = await DialogHandler.GenericYesNoDialog("Er du sikker på du vil Aflyse tavlen for denne bookning?\nDin Booking på rummet vil ikke blive slettet", "Aflys Tavle?", "Ja, Aflys Tavle", "Fortryd");
            // If user wants to delete the booking.
            if (result)
            {
                // Try and run the code, if the code cant run, catch the exception and notify the user that something went wrong.
                try
                {
                    // Gets the selected booking's tavlebooking as an object.
                    TavleBooking _selectedTavleBooking = TavleBookingCatalogSingleton.Instance.TavleBookings.Single(t => t.Booking_Id == Reference.SelectedBooking.Booking_Id);
                    // The async delete method from PersistancyService.
                    PersistancyService.DeleteFromDatabaseAsync("TavleBookings", _selectedTavleBooking.Tavle_Id);
                    // Deletes the selected object from the singleton observable collection, which in turn updates the view.
                    TavleBookingCatalogSingleton.Instance.TavleBookings.Remove(_selectedTavleBooking);
                    //SelectedBooking.TavleStart = null;
                    //SelectedBooking.TavleEnd = null;

                    //Update the viewpage
                    Reference.AflysTavleBtnVisibility = Visibility.Collapsed;
                    Reference.BookTavleBtnVisibility = Visibility.Visible;
                    Reference.SelectedTavleBooking = null;
                    Reference.CheckIfTavleBookingExists();
                }
                catch (Exception)
                {
                    // Informs the user that something went wrong with the deletion of a tavle booking
                    DialogHandler.Dialog("Noget gik galt med aflysning af tavle, kontakt Zealands IT-Helpdesk for mere information.", "Fejl i aflysning");
                }
            }
        }

        /// <summary>
        /// This method books the selected room again tomorrow, if that is possible.
        /// </summary>
        public static async Task BookAgainTomorrowMethodAsync()
        {
            // Retrieves the day after the selected booking date
            DateTime tomorrow = Reference.SelectedBooking.Date.AddDays(1);
            // The copied booking that needs to be inserted into the database with the updated date.
            Booking updatedBooking = new Booking()
            {
                User_Id = Reference.SelectedUser.User_Id,
                Room_Id = Reference.SelectedBooking.Room_Id,
                Date = tomorrow,
                Time_start = Reference.SelectedBooking.BookingStart,
                Time_end = Reference.SelectedBooking.BookingEnd
            };
            // This object will be set to the returned booking that gets posted to the database and later used in the AllBookingsView object
            Booking returnedObj = null;
            // This object is the view object that gets added to the singleton, to that the view will be updated.-
            AllBookingsView viewToAdd = null;
            // Checks how many instances there is of this selectedbooking's specific room.
            var howManyOfThisRoomTomorrowQuery = (from b in AllBookingsViewCatalogSingleton.Instance.AllBookings
                                                  select b).Where(x => Reference.SelectedBooking.Room_Id == x.Room_Id && x.Date == tomorrow).ToList();

            if (howManyOfThisRoomTomorrowQuery.Count == 2)
            {
                // checks if there is any instances that overlaps the selectedbookings's time
                var checkTime = (from b in howManyOfThisRoomTomorrowQuery
                                 select b).Where(x => Reference.SelectedBooking.BookingStart > x.BookingStart && Reference.SelectedBooking.BookingStart < x.BookingEnd || Reference.SelectedBooking.BookingEnd > x.BookingStart && Reference.SelectedBooking.BookingEnd < x.BookingEnd).ToList();
                // If 0 or less
                if (checkTime.Count < 1)
                {
                    // Inserts the selectedbooking into the database and updates the singleton                  
                    returnedObj = await PersistancyService.SaveInsertAsJsonAsync(updatedBooking, "Bookings");
                }
                else
                {
                    // Error message that displays if there already exists a booking in the database that overlaps with the selectedbooking on the day after the selectedbooking date
                    DialogHandler.Dialog("Denne booking kan ikke bookes imorgen\nda den overlapper eksisterende bookninger", "Overlappende Bookninger");
                }
            }
            else
            {
                returnedObj = await PersistancyService.SaveInsertAsJsonAsync(updatedBooking, "Bookings");
            }
            if (returnedObj != null)
            {
                viewToAdd = new AllBookingsView()
                {
                    RoomName = Reference.SelectedBooking.RoomName,
                    Date = tomorrow,
                    Booking_Id = returnedObj.Booking_Id,
                    BookingStart = Reference.SelectedBooking.BookingStart,
                    BookingEnd = Reference.SelectedBooking.BookingEnd,
                    Room_Id = Reference.SelectedBooking.Room_Id,
                    Floor = Reference.SelectedBooking.Floor,
                    No = Reference.SelectedBooking.No,
                    Name = Reference.SelectedBooking.Name,
                    Building_Letter = Reference.SelectedBooking.Building_Letter,
                    Type = Reference.SelectedBooking.Type,
                    User_Id = Reference.SelectedBooking.User_Id
                };
                // Adds the viewToAdd object, to the singleton
                Reference.AllUserBookingsFromSingleton.Add(viewToAdd);
                // Refreshes the singleton, and re-queries the bookings for the selected user
                Reference.RefreshLists();
                // sets the selected booking to the newly added booking
                Reference.SelectedBooking = Reference.AllUserBookingsFromSingleton.Last();
            }
        }

        /// <summary>
        /// Method that checks for preconditions, before calling the BookTavle Method.
        /// </summary>
        public static async Task BookTavleMethodAsync()
        {
            TavleBooking myNewTavleBooking = null;
            if (Reference.SelectedTavleStartTime != TimeSpan.Zero)
            {
                // does selected duration exceed bookingEnd?
                TimeSpan tavleEndTime = Reference.SelectedTavleStartTime.Add(TimeSpan.Parse(Reference.SelectedDuration));
                if (tavleEndTime > Reference.SelectedBooking.BookingEnd)
                {

                    var endTimeExceedsBookingEnd = await DialogHandler.GenericYesNoDialog("Tiden kan ikke overstige sluttiden for denne booking.\nEr du sikker på du vil forsætte?\nDin tavletid vil blive begrænset!", "Begrænset Tavletid!", "Acceptér", "Fortryd");
                    if (endTimeExceedsBookingEnd)
                    {
                        tavleEndTime = Reference.SelectedBooking.BookingEnd;
                        myNewTavleBooking = new TavleBooking() { Booking_Id = Reference.SelectedBooking.Booking_Id, Time_start = Reference.SelectedTavleStartTime, Time_end = tavleEndTime };
                        await Reference.BookTavle(tavleEndTime, myNewTavleBooking);
                    }
                    else
                    {
                        return;
                    }
                }
                else if (Reference.SelectedTavleStartTime < Reference.SelectedBooking.BookingStart)
                {
                    DialogHandler.Dialog("Vælg venligt en anden starttid\nStarttiden kan ikke være mindre end start-tiden for denne booking", "Ugyldig tid");
                    return;
                }
                else
                {
                    myNewTavleBooking = new TavleBooking() { Booking_Id = Reference.SelectedBooking.Booking_Id, Time_start = Reference.SelectedTavleStartTime, Time_end = tavleEndTime };
                    await Reference.BookTavle(tavleEndTime, myNewTavleBooking);
                }
            }
            else
            {
                DialogHandler.Dialog("Vælg venligt en anden starttid\nStarttiden kan ikke være kl 00:00", "Ugyldig tid");
            }
        }

        /// <summary>
        /// Checks if there is any "tavle" bookings for the selected booking. if there is, change the "BookTavleBtnVisibility" to Visible or Collasped respectively
        /// </summary>
        public static void CheckIfTavleBookingExists()
        {
            // Checks if the selected booking is NULL, if not jump to else
            if (Reference.SelectedBooking == null)
            {
                Reference.TavleButtonsEnabled = false;
                Reference.AflysTavleBtnVisibility = Visibility.Collapsed;
                Reference.BookTavleBtnVisibility = Visibility.Collapsed;
            }
            else
            {
                /* 
                 * Queries the ObservableCollection TavleBookings (Which is a copy from the tavlebookings singleton)
                 * and checks if TavleBookings contains a booking id that matches that of the selected booking.
                 * if not, update the visibilities for the buttons "Aflys Tavle" and "Book Tavle" respectively
                */
                if (Reference.SelectedBooking.Type == "Klasselokale")
                {
                    Reference.TavleInkluderetVisibility = Visibility.Collapsed;
                    Reference.TavleCanBeBookedVisibility = Visibility.Visible;
                    var doesUserHaveBookedTavler = (from t in TavleBookingCatalogSingleton.Instance.TavleBookings
                                                    select t).Where(x => x.Booking_Id == Reference.SelectedBooking.Booking_Id).ToList();
                    if (doesUserHaveBookedTavler.Count < 1)
                    {
                        Reference.AflysTavleBtnVisibility = Visibility.Collapsed;
                        Reference.BookTavleBtnVisibility = Visibility.Visible;
                        Reference.TavleButtonsEnabled = true;
                        Reference.SelectedTavleBooking = null;
                    }
                    else
                    {
                        // Sets the queried tavlebooking to be the selectedtavlebooking, for binding in the view
                        foreach (var item in doesUserHaveBookedTavler)
                        {
                            if (Reference.SelectedTavleBooking == null)
                            {
                                Reference.SelectedTavleBooking = item;
                            }
                        }
                        Reference.AflysTavleBtnVisibility = Visibility.Visible;
                        Reference.BookTavleBtnVisibility = Visibility.Collapsed;
                        Reference.TavleButtonsEnabled = false;
                    }
                }
                else
                {
                    Reference.TavleCanBeBookedVisibility = Visibility.Collapsed;
                    Reference.TavleInkluderetVisibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Finds and adds the bookings for the user that is logged in to ObservableCollection<AllBookingsView> AllUserBookingsFromSingleton
        /// </summary>
        /// <param name="userid">The current user's ID</param>
        public static void FindUserBookingsOnId(int userid)
        {
            // Queries the ObservableCollection (Which comes from the singleton that gets ALL the bookings) for the Bookings that is tied to the userid
            var query = (from c in AllBookingsViewCatalogSingleton.Instance.AllBookings
                         where c.User_Id == userid
                         orderby c.Date descending
                         select c).ToList();

            // Adds the queried result to the ObservableCollection
            Reference.AllUserBookingsFromSingleton.Clear();
            foreach (var item in query)
            {
                Reference.AllUserBookingsFromSingleton.Add(item);
            }
        }

        /// <summary>
        /// Refreshes lists, and reloads the singletons for Bookings, and for tavlebookings.
        /// </summary>
        public static void RefreshLists()
        {
            // Bookings singleton
            AllBookingsViewCatalogSingleton.Instance.AllBookings.Clear();
            AllBookingsViewCatalogSingleton.Instance.LoadAllBookingsAsync();
            Reference.AllUserBookingsFromSingleton.Clear();
            foreach (var item in AllBookingsViewCatalogSingleton.Instance.AllBookings.ToList())
            {
                Reference.AllUserBookingsFromSingleton.Add(item);
            }

            // Refresh TavleBookings singleton
            TavleBookingCatalogSingleton.Instance.TavleBookings.Clear();
            TavleBookingCatalogSingleton.Instance.LoadTavleBookingsAsync();
        }

        /// <summary>
        /// When the viewmodel (Page) gets loaded or comes into view set default values on visibilities
        /// </summary>
        public static void OnPageLoadVisibilities()
        {
            Reference.AflysTavleBtnVisibility = Visibility.Collapsed;
            Reference.ElementIsChosenVisibility = Visibility.Collapsed;
            Reference.NoElementsChosenVisibility = Visibility.Visible;
        }

        /// <summary>
        /// Resets the comboxes, and timepicker for tavle booking
        /// </summary>
        public static void ResetSelectedTavleProperties()
        {
            Reference.SelectedTavleStartTime = new TimeSpan(0, 0, 0);
            Reference.SelectedDuration = null;          
        }

        /// <summary>
        /// Steps back to the previous page.
        /// </summary>
        public static void GoBackMethod()
        {
            ((Frame)Window.Current.Content).Navigate(typeof(PageBookRooms));
        }
    }
}
