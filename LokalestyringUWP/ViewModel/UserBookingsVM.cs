using LokalestyringUWP.Common;
using LokalestyringUWP.Handler;
using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;
using LokalestyringUWP.Service;
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

namespace LokalestyringUWP.ViewModel
{
    public class UserBookingsVM : INotifyPropertyChanged
    {
        // Private variable that references AllBookingsView, used in the property "SelectedBooking" to return the selected booking.
        private AllBookingsView _selectedBooking;
        // ObservableCollection of type AllBookingsView that is instantiated in the UserBookingsVM constructor
        public ObservableCollection<AllBookingsView> AllUserBookingsFromSingleton { get; set; }
        // ObservableCollection of type TavleBooking that is instantiated in the viewmodel constructor
        public ObservableCollection<TavleBooking> Tavlebookings { get; set; }


        public UserBookingsVM()
        {
            AllUserBookingsFromSingleton = new ObservableCollection<AllBookingsView>();
            Tavlebookings = new ObservableCollection<TavleBooking>();

            // Create copies of the singleton ObservableCollections
            AllUserBookingsFromSingleton = AllBookingsViewCatalogSingleton.Instance.AllBookings;
            Tavlebookings = TavleBookingCatalogSingleton.Instance.TavleBookings;

            // Instantiates the ICommands properties with a relaycommand
            AflysBookingCommand = new RelayCommand(AflysBookingMethod, null);

            // Loads default visibility states
            OnPageLoadVisibilities();

            // HARD CODED USER ID, for use on the page, as a logged in user.
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
                NoElementsChosenVisibility = Visibility.Collapsed;
                ElementIsChosenVisibility = Visibility.Visible;
                OnPropertyChanged();              
                OnPropertyChanged(nameof(NoElementsChosenVisibility));
                OnPropertyChanged(nameof(ElementIsChosenVisibility));
            } 
        }
        // ICommands that gets bound to XAML-Controllers
        #region Command Properties
        public ICommand BookIgenImorgenCommand { get; set; }
        public ICommand BookTavleCommand { get; set; }
        public ICommand AflysBookingCommand { get; set; }
        public ICommand AflysTavleCommand { get; set; }
        #endregion

        // Properties that is bound to the pageview. When a value is chosen on the page, that value gets put into these properties respectively
        #region Binding Properties
        public TimeSpan TavleBookingStartTime { get; set; }
        public TimeSpan SelectedDuration { get; set; }
        #endregion

        // Visibility Properties that is bound to elements that needs to show and hide on the page view
        #region Visibility Properties
        public Visibility AflysTavleBtnVisibility { get; set; }
        public Visibility BookTavleBtnVisibility { get; set; }
        public Visibility NoElementsChosenVisibility { get; set; }
        public Visibility ElementIsChosenVisibility { get; set; }

        #endregion


        #region Button Command Methods
        /// <summary>
        /// Async method that deletes 
        /// </summary>
        public async void AflysBookingMethod()
        {
            var result = await DialogHandler.GenericYesNoDialog("Er du sikker på du vil slette denne bookning?\nTilhørende tavlebookings vil også blive slettet.", "Slet Bookning?", "Ja, Fjern booking", "Fortryd");
            if (result)
            {
                PersistancyService.DeleteFromDatabaseAsync("Bookings", SelectedBooking.Booking_Id);
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
            var query = (from c in AllUserBookingsFromSingleton
                         select c).Where(c => c.User_Id == userid).ToList();

            // Clears the ObservableCollection
            AllUserBookingsFromSingleton.Clear();

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

                }
                else
                {
                    BookTavleBtnVisibility = Visibility.Collapsed;
                    AflysTavleBtnVisibility = Visibility.Visible;

                }
            }
            // Refreshes the visibility properties
            OnPropertyChanged(nameof(AflysTavleBtnVisibility));
            OnPropertyChanged(nameof(BookTavleBtnVisibility));
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
