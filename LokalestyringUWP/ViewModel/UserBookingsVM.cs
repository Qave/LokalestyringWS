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
            UserBookingHandler = new UserBookingHandler(this);
            // Gets filled with the bookings from the selected user on the UserBookingsOnId Method.
            AllUserBookingsFromSingleton = new ObservableCollection<AllBookingsView>();
            Tavlebookings = new ObservableCollection<TavleBooking>();
            Tavlebookings = TavleBookingCatalogSingleton.Instance.TavleBookings;

            // Instantiates the ICommands properties with a relaycommand
            AflysBookingCommand = new RelayCommand(async () => await AflysBookingMethodAsync(), null);
            AflysTavleCommand = new RelayCommand(async () => await CancelTavleBookingMethodAsync(), null);
            BookTavleCommand = new RelayCommand(async () => await BookTavleMethodAsync(), null);
            BookIgenImorgenCommand = new RelayCommand(async () => await BookAgainTomorrowMethodAsync(), null);
            GoBackCommand = new RelayCommand(GoBackMethod, null);  // DEN ER NULL; FIX DEN xD
            // Loads default visibility states
            OnPageLoadVisibilities();
            // Filters the bookings to only show bookings for the selected user. HARD CODED USER ID, for use on the page, as a logged in user.
            RefreshLists();
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
                // Updates the view when a booking is selected
                NoElementsChosenVisibility = Visibility.Collapsed;
                ElementIsChosenVisibility = Visibility.Visible;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NoElementsChosenVisibility));
                OnPropertyChanged(nameof(ElementIsChosenVisibility));
                ResetSelectedTavleProperties();
            }
        }
        /// <summary>
        /// Returns the user that is logged ind as an User object.
        /// </summary>
        public User SelectedUser { get { return LoginHandler.SelectedUser; } }
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
        public async Task AflysBookingMethodAsync()
        {
            await UserBookingHandler.AflysBookingMethodAsync();
            OnPropertyChanged(nameof(ElementIsChosenVisibility));
            OnPropertyChanged(nameof(NoElementsChosenVisibility));
        }

        /// <summary>
        /// Async method that references the UserBookinghandler and calls the CancelTavleBookingAsync method and updates the view 
        /// </summary>
        public async Task CancelTavleBookingMethodAsync()
        {
            await UserBookingHandler.CancelTavleBookingAsync();
            OnPropertyChanged(nameof(AflysTavleBtnVisibility));
            OnPropertyChanged(nameof(BookTavleBtnVisibility));
            OnPropertyChanged(nameof(SelectedBooking));
            OnPropertyChanged(nameof(SelectedTavleBooking));
        }

        /// <summary>
        /// Async method that references the UserBookinghandler and calls the BookAgainTomorrowMethod and inserts the current selected booking into the database the day after.
        /// </summary>
        public async Task BookAgainTomorrowMethodAsync()
        {
            await UserBookingHandler.BookAgainTomorrowMethodAsync();
        }

        /// <summary>
        /// Async method that references the UserBookinghandler and calls the BookAgainTomorrowMethod and inserts the chosen tavle start and end time and then books the tavle
        /// </summary>
        public async Task BookTavleMethodAsync()
        {
            await UserBookingHandler.BookTavleMethodAsync();
        }

        #endregion
        // Methods for refreshing lists and visibility properties based on userId, the bookfilter method checks if its possible to book a tavle
        #region Refresh and filter methods

        /// <summary>
        /// Method that posts the tavlebooking to the database, after it has checked if its possible in the chosen timespan.
        /// </summary>
        /// <param name="tavleEndTime">The total time for the selected tavlebooking (Comes from the Booktavle Method)</param>
        /// <param name="myTavleBooking">The chosen tavlebooking. This value comes from the SelectedDuration, and SelectedTavleStartTime properties</param>
        /// <returns></returns>
        public async Task BookTavle(TimeSpan tavleEndTime, TavleBooking myNewTavleBooking)
        {
            AllBookingsView tempSelectedBooking = SelectedBooking;

            var doesUserHaveAnyTavleBookingsForThisRoom = (from t in Tavlebookings
                                                           select t).Where(x => x.Booking_Id == SelectedBooking.Booking_Id).ToList();
            if (doesUserHaveAnyTavleBookingsForThisRoom.Count > 0)
            {
                DialogHandler.Dialog("Det er ikke muligt at have flere end 1 tavle\nSlet venligst eksisterende tavler og book derefter igen.", "For mange bookede tavler");
                return;
            }
            else
            {
                if (SelectedBooking.Type == "Klasselokale")
                {
                    var numberOfTavleBookingsForThisRoomOnThatDay = (from t in Tavlebookings
                                                                     join b in AllBookingsViewCatalogSingleton.Instance.AllBookings on t.Booking_Id equals b.Booking_Id
                                                                     select new
                                                                     {
                                                                         BookingId = t.Booking_Id,
                                                                         UserId = b.User_Id,
                                                                         RoomId = b.Room_Id,
                                                                         BookingDate = b.Date,
                                                                         BookingStart = b.BookingStart,
                                                                         BookingEnd = b.BookingEnd,
                                                                         TavleId = t.Tavle_Id,
                                                                         TavleStart = t.Time_start,
                                                                         TavleEnd = t.Time_end

                                                                     }).Where(x => SelectedBooking.Room_Id == x.RoomId && SelectedBooking.Date == x.BookingDate).ToList();
                    if (numberOfTavleBookingsForThisRoomOnThatDay.Count > 0 && numberOfTavleBookingsForThisRoomOnThatDay.Count <= 2)
                    {
                        var checkTavleTime = (from t in numberOfTavleBookingsForThisRoomOnThatDay
                                              select t).Where(x => (SelectedTavleStartTime + TimeSpan.FromSeconds(1)) <= x.TavleEnd && (tavleEndTime - TimeSpan.FromSeconds(1)) >= x.TavleStart).ToList();
                        if (checkTavleTime.Count == 0)
                        {
                            // INSERT 
                            if (await DialogHandler.GenericYesNoDialog("Vil du booke tavlen i dette rum", "Book Tavle?", "Ja", "Fortryd"))
                            {
                                await PersistancyService.SaveInsertAsJsonAsync(myNewTavleBooking, "TavleBookings");
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            DialogHandler.Dialog("Denne tid modstrider en anden tavle booking\nVælg venligst en tidligere eller senere tid", "Modstridende tider");
                        }
                    }
                    else
                    {
                        //INSERT

                        if (await DialogHandler.GenericYesNoDialog("Vil du booke tavlen i dette rum", "Book Tavle?", "Ja", "Fortryd"))
                        {
                            await PersistancyService.SaveInsertAsJsonAsync(myNewTavleBooking, "TavleBookings");
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            RefreshLists();
            SelectedBooking = tempSelectedBooking;
            
        }

        /// <summary>
        /// Find and add the bookings for the user that is logged in to ObservableCollection<AllBookingsView> AllUserBookingsFromSingleton
        /// </summary>
        /// <param name="userid">The current user's ID</param>
        public void UserBookingsOnId(int userid)
        {
            // Queries the ObservableCollection (Which comes from the singleton that gets ALL the bookings) for the Bookings that is tied to the userid
            var query = (from c in AllBookingsViewCatalogSingleton.Instance.AllBookings
                         where c.User_Id == userid
                         orderby c.Date ascending
                         select c).ToList();

            // Adds the queried result to the ObservableCollection
            AllUserBookingsFromSingleton.Clear();
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
            //// Checks if the selected booking is NULL, if not jump to else
            //if (SelectedBooking == null)
            //{
            //    TavleButtonsEnabled = false;
            //    AflysTavleBtnVisibility = Visibility.Collapsed;
            //    BookTavleBtnVisibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    /* 
            //     * Queries the ObservableCollection TavleBookings (Which is a copy from the tavlebookings singleton)
            //     * and checks if TavleBookings contains a booking id that matches that of the selected booking.
            //     * if not, update the visibilities for the buttons "Aflys Tavle" and "Book Tavle" respectively
            //    */
            //    if (SelectedBooking.Type == "Klasselokale")
            //    {
            //        TavleInkluderetVisibility = Visibility.Collapsed;
            //        TavleCanBeBookedVisibility = Visibility.Visible;
            //        var doesUserHaveBookedTavler = (from t in TavleBookingCatalogSingleton.Instance.TavleBookings
            //                                        select t).Where(x => x.Booking_Id == SelectedBooking.Booking_Id).ToList();
            //        if (doesUserHaveBookedTavler.Count < 1)
            //        {
            //            AflysTavleBtnVisibility = Visibility.Collapsed;
            //            BookTavleBtnVisibility = Visibility.Visible;
            //            TavleButtonsEnabled = true;
            //            _selectedTavleBooking = null;
            //        }
            //        else
            //        {
            //            // Sets the queried tavlebooking to be the selectedtavlebooking, for binding in the view
            //            foreach (var item in doesUserHaveBookedTavler)
            //            {
            //                if (_selectedTavleBooking == null)
            //                {
            //                    _selectedTavleBooking = item;
            //                }
            //            }
            //            AflysTavleBtnVisibility = Visibility.Visible;
            //            BookTavleBtnVisibility = Visibility.Collapsed;
            //            TavleButtonsEnabled = false;
            //        }
            //    }
            //    else
            //    {
            //        TavleCanBeBookedVisibility = Visibility.Collapsed;
            //        TavleInkluderetVisibility = Visibility.Visible;
            //    }
            //}

            // Refreshes the visibility properties
            OnPropertyChanged(nameof(AflysTavleBtnVisibility));
            OnPropertyChanged(nameof(BookTavleBtnVisibility));
            OnPropertyChanged(nameof(TavleInkluderetVisibility));
            OnPropertyChanged(nameof(TavleCanBeBookedVisibility));
            OnPropertyChanged(nameof(TavleButtonsEnabled));
            OnPropertyChanged(nameof(SelectedTavleBooking));
        }

        /// <summary>
        /// Refreshes lists, and reloads the singletons for Bookings, and for tavlebookings.
        /// </summary>
        public void RefreshLists()
        {
            // Bookings singleton
            AllBookingsViewCatalogSingleton.Instance.AllBookings.Clear();
            AllBookingsViewCatalogSingleton.Instance.LoadAllBookingsAsync();
            AllUserBookingsFromSingleton.Clear();
            foreach (var item in AllBookingsViewCatalogSingleton.Instance.AllBookings.ToList())
            {
                AllUserBookingsFromSingleton.Add(item);
            }

            // Refresh TavleBookings singleton
            TavleBookingCatalogSingleton.Instance.TavleBookings.Clear();
            TavleBookingCatalogSingleton.Instance.LoadTavleBookingsAsync();

            // Sets the bookings for the selected user
            UserBookingsOnId(SelectedUser.User_Id);
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

        #endregion
        /// <summary>
        /// Resets the comboxes, and timepicker for tavle booking
        /// </summary>
        public void ResetSelectedTavleProperties()
        {
            SelectedTavleStartTime = new TimeSpan(0,0,0);
            SelectedDuration = null;
            OnPropertyChanged(nameof(SelectedTavleStartTime));
            OnPropertyChanged(nameof(SelectedDuration));
        }

        /// <summary>
        /// Steps back to the previous page.
        /// </summary>
        public void GoBackMethod()
        {
            ((Frame)Window.Current.Content).Navigate(typeof(PageBookRooms));
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