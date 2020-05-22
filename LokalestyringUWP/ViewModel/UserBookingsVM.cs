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
        #region Backing Fields
        // Private variable that references AllBookingsView, used in the property "SelectedBooking" to return the selected booking.
        private AllBookingsView _selectedBooking;
        // Variable that stores the selectedTavleBooking value.
        private TavleBooking _selectedTavleBooking;
        // Variable to store the selected tavle start time value.
        private TimeSpan _selectedTavleStartTime;
        // Variable to store the Selected duration value.
        private string _selectedDuration;
        #endregion

        // ObservableCollection of type AllBookingsView that is instantiated in the UserBookingsVM constructor.
        public ObservableCollection<AllBookingsView> AllUserBookingsFromSingleton { get; set; }
        // ObservableCollection of type TavleBooking that is instantiated in the viewmodel constructor
        public ObservableCollection<TavleBooking> Tavlebookings { get; set; }

        /// <summary>
        /// The UserBookingsVM ViewModel, instantiates the ICommand properties, it also refreshes the view, and reloads the lists to use in the view.
        /// </summary>
        public UserBookingsVM()
        {

            // Initialize first
            UserBookingHandler = new UserBookingHandler(this);
            // Gets filled with the bookings from the selected user on the UserBookingsOnId Method.
            AllUserBookingsFromSingleton = new ObservableCollection<AllBookingsView>();
            Tavlebookings = new ObservableCollection<TavleBooking>();
            Tavlebookings = TavleBookingCatalogSingleton.Instance.TavleBookings;

            // Instantiates the ICommands properties with a relaycommand
            AflysBookingCommand = new RelayCommand(async () => await CancelBookingMethodAsync(), null);
            AflysTavleCommand = new RelayCommand(async () => await CancelTavleBookingMethodAsync(), null);
            BookTavleCommand = new RelayCommand(async () => await UserBookingHandler.BookTavleMethodAsync(), null);
            BookIgenImorgenCommand = new RelayCommand(async () => await UserBookingHandler.BookAgainTomorrowMethodAsync(), null);
            GoBackCommand = new RelayCommand(UserBookingHandler.GoBackMethod, null);  // DEN ER NULL; FIX DEN xD
            // Loads default visibility states
            UserBookingHandler.OnPageLoadVisibilities();
            // Filters the bookings to only show bookings for the selected user. HARD CODED USER ID, for use on the page, as a logged in user.
            UserBookingHandler.RefreshLists();
        }

        public UserBookingHandler UserBookingHandler { get; set; }

        // Properties that is bound to the pageview. When a value is chosen on the page, that value gets put into these properties respectively
        #region Binding Properties

        public TimeSpan SelectedTavleStartTime { get { return _selectedTavleStartTime; } set { _selectedTavleStartTime = value; OnPropertyChanged(); } }
        public string SelectedDuration { get { return _selectedDuration; } set { _selectedDuration = value; OnPropertyChanged(); } }
        public List<string> PossibleDurations
        {
            get
            {
                return new List<string>
                {
                   "00:30:00",
                   "01:00:00",
                   "01:30:00",
                   "02:00:00"
                };
            }
            set
            {
                OnPropertyChanged();
            }
        }
        public bool TavleButtonsEnabled { get; set; }
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
                ResetSelectedTavleProperties();
                // Updates the view when a booking is selected
                NoElementsChosenVisibility = Visibility.Collapsed;
                ElementIsChosenVisibility = Visibility.Visible;
                SelectedTavleStartTime = SelectedBooking.BookingStart;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NoElementsChosenVisibility));
                OnPropertyChanged(nameof(ElementIsChosenVisibility));
            }
        }
        /// <summary>
        /// Returns the user that is logged ind as an User object.
        /// </summary>
        public User SelectedUser { get { return LoginHandler.SelectedUser; } }

        /// <summary>
        /// Returns the selected tavlebooking. Default on load = null
        /// </summary>
        public TavleBooking SelectedTavleBooking
        {
            get
            {
                return _selectedTavleBooking;
            }
            set 
            {
                _selectedTavleBooking = value;
                OnPropertyChanged();
            }
        }

        #endregion

        // ICommands that gets bound to XAML-Controllers
        #region Command Properties

        public ICommand BookIgenImorgenCommand { get; set; }
        public ICommand BookTavleCommand { get; set; }
        public ICommand AflysBookingCommand { get; set; }
        public ICommand AflysTavleCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

        #endregion
        // Visibility Properties that is bound to elements that needs to show and hide on the page view
        #region Visibility Properties

        public Visibility AflysTavleBtnVisibility { get; set; }
        public Visibility BookTavleBtnVisibility { get; set; }
        public Visibility NoElementsChosenVisibility { get; set; }
        public Visibility ElementIsChosenVisibility { get; set; }
        public Visibility TavleInkluderetVisibility { get; set; }
        public Visibility TavleCanBeBookedVisibility { get; set; }

        #endregion

        // Methods for binding the corresponding ICommand property.
        #region Button Command Methods

        /// <summary>
        /// Async method that references the UserBookinghandler and calls the cancelBookingMethod and updates the view
        /// </summary>
        public async Task CancelBookingMethodAsync()
        {
            await UserBookingHandler.CancelBookingMethodAsync();
            OnPropertyChanged(nameof(ElementIsChosenVisibility));
            OnPropertyChanged(nameof(NoElementsChosenVisibility));
        }

        /// <summary>
        /// Async method that references the UserBookinghandler and calls the CancelTavleBookingAsync method and updates the view 
        /// </summary>
        public async Task CancelTavleBookingMethodAsync()
        {
            await UserBookingHandler.CancelTavleBookingMethodAsync();
            OnPropertyChanged(nameof(AflysTavleBtnVisibility));
            OnPropertyChanged(nameof(BookTavleBtnVisibility));
            OnPropertyChanged(nameof(SelectedBooking));
            OnPropertyChanged(nameof(SelectedTavleBooking));
        }

        #endregion
        // Methods for refreshing lists and visibility properties based on userId, the bookfilter method checks if its possible to book a tavle
        #region Refresh and filter methods

        /// <summary>
        /// Method that calls the UserBookingHandler and calls the corresponding methodname, and updates the view after the call
        /// </summary>
        public void CheckIfTavleBookingExists()
        {
            UserBookingHandler.CheckIfTavleBookingExists();
            // Refreshes the visibility properties
            OnPropertyChanged(nameof(AflysTavleBtnVisibility));
            OnPropertyChanged(nameof(BookTavleBtnVisibility));
            OnPropertyChanged(nameof(TavleInkluderetVisibility));
            OnPropertyChanged(nameof(TavleCanBeBookedVisibility));
            OnPropertyChanged(nameof(TavleButtonsEnabled));
            OnPropertyChanged(nameof(SelectedTavleBooking));
        }

        #endregion
        /// <summary>
        /// Resets the comboxes, and timepicker for tavle booking and updates the viewbound properties
        /// </summary>
        public void ResetSelectedTavleProperties()
        {
            UserBookingHandler.ResetSelectedTavleProperties();
            OnPropertyChanged(nameof(SelectedTavleStartTime));
            OnPropertyChanged(nameof(SelectedDuration));
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