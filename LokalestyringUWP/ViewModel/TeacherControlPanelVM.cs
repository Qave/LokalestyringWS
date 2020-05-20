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
        #region Properties + Instance + Commands

        private AllBookingsView _bookingIsSelect;
        public ObservableCollection<AllBookingsView> BookingList { get; set; }
        public Visibility TeacherDeleteBtnVisibility { get; set; } = Visibility.Collapsed;
        public TeacherPanelBookingHandler TeacherHandlerRef { get; set; }
        public DateTime InputDate { get; set; } = DateTime.Now.AddDays(5);
        public TimeSpan InuputTimeStart { get; set; } = TimeSpan.Parse("12:00:00");
        public TimeSpan InputTimeEnd { get; set; } = TimeSpan.Parse("18:00:00");
        public User SelectedUser { get { return LoginHandler.SelectedUser; } }

        public ICommand GoBackCommand { get; set; }
        public ICommand NavigateCommand { get; set; }
        public RelayCommand StealThisRoomCommand { get; set; }


        #endregion



        public TeacherControlPanelVM()
        {
            TeacherHandlerRef = new TeacherPanelBookingHandler(this);
            BookingList = new ObservableCollection<AllBookingsView>();
            TeacherHandlerRef.ResetList();
            StealThisRoomCommand = new RelayCommand(RoomSnatch, null);
            NavigateCommand = new RelayCommand(ChangePage, null);
            GoBackCommand = new RelayCommand(GoBackMethod, null);

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

        public void RoomSnatch()
        {
            TeacherHandlerRef.TeacherStealsBookingMethod();
        }

        public bool BookingIsSelectedCheck()
        {
            return (_bookingIsSelect != null);
        }
        #endregion

        public void TeacherCancelBookingBtnVisibility()
        {
            if (LoginHandler.SelectedUser.Teacher == true)
            {
                TeacherDeleteBtnVisibility = Visibility.Visible;
            }
        }

        public void ChangePage()
        {
            ((Frame) Window.Current.Content).Navigate(typeof(PageTeacherControlPanel));
        }

        public void GoBackMethod()
        {
            ((Frame)Window.Current.Content).Navigate(typeof(PageLocations));
        }


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
