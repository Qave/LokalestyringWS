using LokalestyringUWP.Common;
using LokalestyringUWP.Handler;
using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;
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
using LokalestyringUWP.View;

namespace LokalestyringUWP.ViewModel
{
    public class BookRoomsVM : INotifyPropertyChanged
    {
        private DateTimeOffset _selectedDate;
        private TimeSpan _timeStart;
        private TimeSpan _timeEnd;
        private RoomsView _selectedRoomsView;
        public ICommand BookSelectedRoomCommand { get; set; }
        public ICommand FilterSearchCommand => CommandHandler.FilterSearchCommand;
        public ICommand GoBackCommand => CommandHandler.GoBackCommand;
        public string selectedLocation => LocationsVM.SelectedLocation.City;
        
        public BookRoomsVM()
        {
            RoomHandler = new RoomHandler(this); //SKAL INITIALISERES FØRST
            Date = DateTimeOffset.Now;
            Date = Date.Date;
            TimeStart = DateTime.Now.TimeOfDay;
            TimeEnd = DateTime.Now.TimeOfDay + TimeSpan.FromHours(4);
            BookSelectedRoomCommand = new RelayCommand(RoomHandler.CreateBooking, null);
            CommandHandler.BookSelectedRoomCommand = new RelayCommand(DialogHandler.ConfirmBookingDialog, null);
            CommandHandler.FilterSearchCommand = new RelayCommand(RoomHandler.FilterSearchMethod, null);
            CommandHandler.GoBackCommand = new RelayCommand(RoomHandler.GoBackMethod, null);
            RoomList = new ObservableCollection<RoomsView>();
            OnPropertyChanged(nameof(RoomList));
            ResettedList = new ObservableCollection<RoomsView>();
            SelectedRoomtypeFilter = RoomtypeList[0];
            SelectedBuildingFilter = BuildingList[0];
            RoomHandler.FilterSearchMethod();
        }

        public ObservableCollection<RoomsView> ResettedList { get; set; }
        public ObservableCollection<RoomsView> RoomList { get; set; }

        public RoomHandler RoomHandler { get; set; }
        public void RoomIsSelected()
        {
            RoomIsSelectedCheck = true;
            OnPropertyChanged(nameof(RoomIsSelectedCheck));
        }
        public bool RoomIsSelectedCheck { get; set; }
        public TimeSpan TimeStart 
        {
            get
            {
                return _timeStart;
            }
            set
            {
                _timeStart = value;
            } 
        }
        public TimeSpan TimeEnd 
        { 
            get
            {
                return _timeEnd;
            }
            set
            {
                _timeEnd = value;
            }
        }
        public DateTimeOffset Date 
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }
        public RoomsView SelectedRoomsView
        {
            get { return _selectedRoomsView; }
            set
            {
                _selectedRoomsView = value;
                RoomIsSelected();
                OnPropertyChanged();
            }
        }
        public string SelectedBuildingFilter { get; set; }
        public string SelectedRoomtypeFilter { get; set; }


        public List<string> RoomtypeList
        {
            get
            {
                return new List<string>
                {
                   "Alle", "Lille", "Medium", "Klasselokale"
                };
            }
        }

        public List<string> BuildingList
        {
            get
            {
                return new List<string>
                {
                    "Alle", "A", "B", "C", "D"
                };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
