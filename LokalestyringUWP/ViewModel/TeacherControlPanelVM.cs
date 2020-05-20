using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using LokalestyringUWP.Annotations;
using LokalestyringUWP.Common;
using LokalestyringUWP.Handler;
using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;

namespace LokalestyringUWP.ViewModel
{
    public class TeacherControlPanelVM : INotifyPropertyChanged
    {
        #region Properties + Instance + Commands

        private AllBookingsView _bookingIsSelect;
        public ObservableCollection<AllBookingsView> BookingList { get; set; }
        public Visibility TeacherDeleteBtnVisibility { get; set; } = Visibility.Collapsed;
        public TeacherPanelBookingHandler TeacherHandlerRef { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        

        public RelayCommand BookThisRoomCommand { get; set; }


        #endregion



        public TeacherControlPanelVM()
        {
            TeacherHandlerRef = new TeacherPanelBookingHandler(this);
            BookingList = new ObservableCollection<AllBookingsView>();
            TeacherHandlerRef.ResetList();
            //BookThisRoomCommand = new RelayCommand(RoomHandlerRef.TeacherCancelBookingMethod,null);
        }


        #region SelectedBooking
        public AllBookingsView BookingIsSelected
        {
            get { return _bookingIsSelect; }
            set
            {
                _bookingIsSelect = value;
                OnPropertyChanged(nameof(BookingIsSelectedCheck));
                BookThisRoomCommand.RaiseCanExecuteChanged();
            }
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
