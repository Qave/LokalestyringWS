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

namespace LokalestyringUWP.ViewModel
{
    public class TeacherControlPanelVM : INotifyPropertyChanged
    {
        public ObservableCollection<AllBookingsView> BookingList { get; set; }
        public Visibility TeacherDeleteBtnVisibility { get; set; } = Visibility.Collapsed;
        public RoomHandler RoomHandlerRef { get; set; }
        private AllBookingsView _bookingIsSelect;

        public RelayCommand BookThisRoomCommand { get; set; }

       

        public TeacherControlPanelVM()
        {
            BookingList = new ObservableCollection<AllBookingsView>();
            //BookThisRoomCommand = new RelayCommand(RoomHandlerRef.TeacherCancelBookingMethod,null);
        }


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


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
