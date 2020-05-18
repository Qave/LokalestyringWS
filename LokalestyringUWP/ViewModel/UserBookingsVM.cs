using LokalestyringUWP.Common;
using LokalestyringUWP.Handler;
using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;
using LokalestyringUWP.Service;
using LokalestyringUWP.View;
using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LokalestyringUWP.ViewModel
{
    public class UserBookingsVM : INotifyPropertyChanged
    {
        // Private variable that references AllBookingsView, used in the property "SelectedBooking" to return the selected booking.
        private AllBookingsView _selectedBooking;
        // Should get all the bookings in the "AllBookingsView", this is ALL the bookings in the database. Instantiated in the constructor
            // public ObservableCollection<AllBookingsView> AllBookingsList { get; set; }
        // ObservableCollection of type AllBookingsView that is instantiated in the UserBookingsVM constructor.
        public ObservableCollection<AllBookingsView> AllUserBookingsFromSingleton { get; set; }
        // ObservableCollection of type TavleBooking that is instantiated in the viewmodel constructor
        public ObservableCollection<TavleBooking> Tavlebookings { get; set; }
    

        public UserBookingsVM()
        {
            // Gets filled with the bookings from the selected user on the UserBookingsOnId Method.
            AllUserBookingsFromSingleton = new ObservableCollection<AllBookingsView>();
            Tavlebookings = new ObservableCollection<TavleBooking>();

            // Refreshes the AllBookingsList with the list from the singleton
            //RefreshList();
            Tavlebookings = TavleBookingCatalogSingleton.Instance.TavleBookings;

            // Instantiates the ICommands properties with a relaycommand
            AflysBookingCommand = new RelayCommand(AflysBookingMethod, null);
            AflysTavleCommand = new RelayCommand(AflysTavleBookingMethod, null);
            BookTavleCommand = new RelayCommand(BookTavleMethod, null);
            GoBackCommand = new RelayCommand(GoBackMethod ,null);  // DEN ER NULL; FIX DEN xD
            BookIgenImorgenCommand = new RelayCommand(BookIgenImorgenMethod, null);

            // Loads default visibility states
            OnPageLoadVisibilities();

            // Filters the bookings to only show bookings for the selected user. HARD CODED USER ID, for use on the page, as a logged in user.
            UserBookingsOnId(1);
        }
        /// <summary>
        /// Returns the selected Booking as type: AllBookingsView
        /// </summary>
        public AllBookingsView SelectedBooking 
        { 
            get 
            { return _selectedBooking; } 
            set
            {
                _selectedBooking = value;

                CheckIfTavleBookingExists();
                // Updates the view when a booking is selected
                NoElementsChosenVisibility = Visibility.Collapsed;
                ElementIsChosenVisibility = Visibility.Visible;
                OnPropertyChanged();              
                OnPropertyChanged(nameof(NoElementsChosenVisibility));
                OnPropertyChanged(nameof(ElementIsChosenVisibility));
            } 
        }

        public User SelectedUser { get { return LoginHandler.SelectedUser; } }

        // ICommands that gets bound to XAML-Controllers
        #region Command Properties
        public ICommand BookIgenImorgenCommand { get; set; }
        public ICommand BookTavleCommand { get; set; }
        public ICommand AflysBookingCommand { get; set; }
        public ICommand AflysTavleCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        #endregion

        // Properties that is bound to the pageview. When a value is chosen on the page, that value gets put into these properties respectively
        #region Binding Properties
        public TimeSpan SelectedTavleStartTime { get; set; }
        public TimeSpan SelectedDuration { get; set; }

        #endregion

        // Visibility Properties that is bound to elements that needs to show and hide on the page view
        #region Visibility Properties
        public Visibility AflysTavleBtnVisibility { get; set; }
        public Visibility BookTavleBtnVisibility { get; set; }
        public Visibility NoElementsChosenVisibility { get; set; }
        public Visibility ElementIsChosenVisibility { get; set; }

        public bool TavleButtonsEnabled { get; set; }
        #endregion


        #region Button Command Methods

        public void GoBackMethod()
        {
            ((Frame)Window.Current.Content).Navigate(typeof(PageBookRooms));
        }

        /// <summary>
        /// Method for inserting a booking into the database.
        /// </summary>
        public async void BookTavleMethod()
        {
            TimeSpan totalTavleTime = SelectedTavleStartTime.Add(SelectedDuration);

            if (true)
            {

            }

            //Selected Duration = 2 timer
            // TavlebookingStartTime = 09:00:00
            if (SelectedBooking.Type == "Klasselokale")
            {
                var query = (from t in Tavlebookings
                             join b in AllUserBookingsFromSingleton on t.Booking_Id equals b.Booking_Id
                             select b);
                foreach (var item in query)
                {

                }
            }
            //PersistancyService.SaveInsertAsJsonAsync(new TavleBooking() {Booking_Id = SelectedBooking.Booking_Id, Time_start = SelectedTavleStartTime, Time_end }, "TavleBookings");

        }
        /// <summary>
        /// This method books the selected booking/room, again tomorrow, is that is possible.
        /// </summary>
        public async void BookIgenImorgenMethod()
        {
            // Retrieves the day after the selectedbooking date.
            DateTime tomorrow = SelectedBooking.Date.AddDays(1);
            // The copied booking that needs to be inserted into the database with the updated date.
            Booking updatedBooking = new Booking()
            {
                User_Id = 1,
                Room_Id = SelectedBooking.Room_Id,
                Date = tomorrow,
                Time_start = (TimeSpan)SelectedBooking.BookingStart,
                Time_end = (TimeSpan)SelectedBooking.BookingEnd
            };

            // Checks how many instances there is of this selectedbooking's specific room.
            var howManyOfThisRoomTomorrowQuery = (from b in AllBookingsViewCatalogSingleton.Instance.AllBookings
                                        select b).Where(x => SelectedBooking.Room_Id == x.Room_Id && x.Date == tomorrow).ToList();

            if (howManyOfThisRoomTomorrowQuery.Count > 0)
            {
                // checks if there is any instances that overlaps the selectedbookings's time
                var checkTime = (from b in howManyOfThisRoomTomorrowQuery
                                 select b).Where(x => SelectedBooking.BookingStart > x.BookingStart && SelectedBooking.BookingStart < x.BookingEnd || SelectedBooking.BookingEnd > x.BookingStart && SelectedBooking.BookingEnd < x.BookingEnd).ToList();
                // If 0
                if (checkTime.Count < 1)
                {
                    // Inserts the selectedbooking into the database and updates the singleton                  
                    PersistancyService.SaveInsertAsJsonAsync(updatedBooking, "Bookings");
                }
                else
                {
                    // Error message that displays if there already exists a booking in the database that overlaps with the selectedbooking on the day after the selectedbooking date
                    DialogHandler.Dialog("Denne booking kan ikke bookes imorgen\nda den overlapper eksisterende bookninger", "Overlappende Bookninger");
                }
            }
            else
            {
                PersistancyService.SaveInsertAsJsonAsync(updatedBooking, "Bookings");
                RefreshList();
            }
        }

        public async void RefreshList()
        {

            //AllBookingsViewCatalogSingleton.Instance.AllBookings.Clear();
            //AllBookingsViewCatalogSingleton.Instance.LoadAllBookingsAsync();
            //AllUserBookingsFromSingleton.Clear();
            //foreach (var item in AllBookingsViewCatalogSingleton.Instance.AllBookings.ToList())
            //{
            //    AllUserBookingsFromSingleton.Add(item);
            //}
            UserBookingsOnId(1);


        }

            //}
            /// <summary>
            /// Async method that calls the async delete method from the persistancyService that deletes the selected booking from the database
            /// </summary>
            public async void AflysBookingMethod()
        {
            // Checks if the user wants to delete the booking, or not
            var result = await DialogHandler.GenericYesNoDialog("Er du sikker på du vil Aflyse denne bookning?\nTilhørende tavlebookings vil også blive Aflyst.", "Aflys Bookning?", "Ja, Aflys booking", "Fortryd");
            // If user wants to delete the booking.
            if (result)
            {
                // The async delete method from PersistancyService.
                PersistancyService.DeleteFromDatabaseAsync("Bookings", SelectedBooking.Booking_Id);

                // Deletes the selected object from the singleton observable collection
                AllUserBookingsFromSingleton.Remove(SelectedBooking);
                // Update the view
                ElementIsChosenVisibility = Visibility.Collapsed;
                NoElementsChosenVisibility = Visibility.Visible;
                OnPropertyChanged(nameof(ElementIsChosenVisibility));
                OnPropertyChanged(nameof(NoElementsChosenVisibility));
            }            
        }
        /// <summary>
        /// Async method that calls the async delete method from the persistancyService that deletes the selected booking's Tavle booking from the database 
        /// </summary>
        public async void AflysTavleBookingMethod()
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
                    TavleBooking _selectedTavleBooking = TavleBookingCatalogSingleton.Instance.TavleBookings.Single(t => t.Booking_Id == SelectedBooking.Booking_Id);
                    // The async delete method from PersistancyService.
                    PersistancyService.DeleteFromDatabaseAsync("TavleBookings", _selectedTavleBooking.Tavle_Id);
                    // Deletes the selected object from the singleton observable collection, which in turn updates the view.
                    TavleBookingCatalogSingleton.Instance.TavleBookings.Remove(_selectedTavleBooking);
                    SelectedBooking.TavleStart = null;
                    SelectedBooking.TavleEnd = null;

                    //Update the viewpage
                    AflysTavleBtnVisibility = Visibility.Collapsed;
                    BookTavleBtnVisibility = Visibility.Visible;
                    OnPropertyChanged(nameof(AflysTavleBtnVisibility));
                    OnPropertyChanged(nameof(BookTavleBtnVisibility));
                    OnPropertyChanged(nameof(SelectedBooking));
                }
                catch (Exception)
                {
                    DialogHandler.Dialog("Noget gik galt med aflysning af tavle, kontakt Zealands IT-Helpdesk for mere information.", "Fejl i aflysning");
                }            
            }
        }

        #endregion
        /// <summary>
        /// Find and add the bookings for the user that is logged in to ObservableCollection<AllBookingsView> AllUserBookingsFromSingleton
        /// </summary>
        /// <param name="userid">The current user's ID</param>
        public void UserBookingsOnId(int userid)
        {
            // Queries the ObservableCollection (Which comes from the singleton that gets ALL the bookings) for the Bookings that is tied to the userid
            var query = (from c in AllBookingsViewCatalogSingleton.Instance.AllBookings
                         select c).Where(c => c.User_Id == userid).ToList();
            // Adds the queried result to the ObservableCollection
            foreach (var item in query)
            {
                AllUserBookingsFromSingleton.Add(item);
            }
        }

        /// <summary>
        /// Checks if there is any "tavle" bookings for the selected booking. if there is, change the "BookTavleBtnVisibility" to Visible or Collasped respectively
        /// </summary>
        /// <returns>Visiblity</returns>
        public void CheckIfTavleBookingExists()
        {
            // Checks if the selected booking is NULL, if not jump to else
            if (SelectedBooking == null)
            {
                TavleButtonsEnabled = false;
                AflysTavleBtnVisibility = Visibility.Collapsed;
                BookTavleBtnVisibility = Visibility.Collapsed;
            }
            else
            {
                /* 
                 * Queries the ObservableCollection TavleBookings (Which is a copy from the tavlebookings singleton)
                 * and checks if TavleBookings contains a booking id that matches that of the selected booking.
                 * if not, update the visibilities for the buttons "Aflys Tavle" and "Book Tavle" respectively
                */
                var query = (from t in Tavlebookings
                             select t).Where(x => x.Booking_Id == SelectedBooking.Booking_Id).ToList();
                if (query.Count < 1)
                {
                    AflysTavleBtnVisibility = Visibility.Collapsed;
                    BookTavleBtnVisibility = Visibility.Visible;
                    TavleButtonsEnabled = true;

                }
                else
                {
                    AflysTavleBtnVisibility = Visibility.Visible;
                    BookTavleBtnVisibility = Visibility.Collapsed;
                    TavleButtonsEnabled = false;

                }
            }
            // Refreshes the visibility properties
            OnPropertyChanged(nameof(AflysTavleBtnVisibility));
            OnPropertyChanged(nameof(BookTavleBtnVisibility));
            OnPropertyChanged(nameof(TavleButtonsEnabled));
        }



        /// <summary>
        /// When the viewmodel (Page) gets loaded or comes into view set default values on visibilities
        /// </summary>
        public void OnPageLoadVisibilities()
        {
            AflysTavleBtnVisibility = Visibility.Collapsed;
            ElementIsChosenVisibility = Visibility.Collapsed;
            NoElementsChosenVisibility = Visibility.Visible;
        }

        #region INotifyPropertyChanged interface implementation
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Refreshes the property on the pageview.
        /// </summary>
        /// <param name="propertyName">You can specify the property to update when using "nameof(propertyName)" as a parameter</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
