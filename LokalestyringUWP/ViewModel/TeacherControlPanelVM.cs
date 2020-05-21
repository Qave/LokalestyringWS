using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LokalestyringUWP.Annotations;
using LokalestyringUWP.Common;
using LokalestyringUWP.Handler;
using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;
using LokalestyringUWP.View;

namespace LokalestyringUWP.ViewModel
{
    public class TeacherControlPanelVM : INotifyPropertyChanged
    {
        #region Properties+ References + Instance + Commands

        private AllBookingsView _bookingIsSelect;
        public ObservableCollection<AllBookingsView> BookingList { get; set; }
        public Visibility TeacherDeleteBtnVisibility { get; set; } = Visibility.Collapsed;
        public TeacherPanelBookingHandler TeacherHandlerRef { get; set; }
        public DateTimeOffset InputDate { get; set; }
        public TimeSpan InputTimeStart { get; set; }
        public TimeSpan InputTimeEnd { get; set; }
        public User SelectedUser { get { return LoginHandler.SelectedUser; } }

        public ICommand GoBackCommand { get; set; }
        public ICommand NavigateCommand { get; set; }
        public RelayCommand StealThisRoomCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        #endregion

        public TeacherControlPanelVM()
        {
            TeacherHandlerRef = new TeacherPanelBookingHandler(this);
            BookingList = new ObservableCollection<AllBookingsView>();
            StealThisRoomCommand = new RelayCommand(RoomSnatch, BookingIsSelectedCheck);
            NavigateCommand = new RelayCommand(ChangePage, null);
            GoBackCommand = new RelayCommand(GoBackMethod, null);
            FilterCommand = new RelayCommand(TeacherHandlerRef.FilterMethod,null);
            InputDate = DateTimeOffset.Now.Date;
            InputTimeStart = DateTime.Now.TimeOfDay;
            InputTimeEnd = InputTimeStart + TimeSpan.FromHours(2);
            TeacherHandlerRef.ResetList();

        }
        /// <summary>
        /// RelayCommand expects a method of type Void, so RoomSnatch which is a void, calls TeacherStealsBooking indirectly instead
        /// </summary>
        public void RoomSnatch()
        {
            TeacherHandlerRef.TeacherStealsBookingMethod();
        }


        #region SelectedBooking
        public AllBookingsView BookingIsSelected
        {
            get { return _bookingIsSelect; }
            set
            {
                _bookingIsSelect = value;
                OnPropertyChanged(nameof(BookingIsSelectedCheck));
                StealThisRoomCommand.RaiseCanExecuteChanged();
            }
        }
        /// <summary>
        /// Checks if Booking is selected, if it is selected returns true
        /// </summary>
        /// <returns></returns>
        public bool BookingIsSelectedCheck()
        {
            return (_bookingIsSelect != null);
        }
        #endregion

        #region Redirect Methods

        /// <summary>
        /// Changes page to PageTeacherControlPanel
        /// </summary>
        public void ChangePage()
        {
            ((Frame)Window.Current.Content).Navigate(typeof(PageTeacherControlPanel));
        }
        /// <summary>
        /// Changes page to PageBookRooms
        /// </summary>
        public void GoBackMethod()
        {
            ((Frame)Window.Current.Content).Navigate(typeof(PageBookRooms));
        } 
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
