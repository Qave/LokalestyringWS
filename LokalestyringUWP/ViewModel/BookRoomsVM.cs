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
        #region Instance Fields
        private RoomsView _selectedRoomsView;
        private TimeSpan _selectedStartTime;
        private TimeSpan _selectedEndTime;
        private string _selectedBuilding;
        private string _selectedRoomType;
        private DateTimeOffset _selectedDate;
        #endregion


        public BookRoomsVM()
        {
            RoomHandler = new RoomHandler(this); //SKAL INITIALISERES FØRST
            RoomList = new ObservableCollection<RoomsView>();
            Date = DateTimeOffset.Now.Date;
            TimeStart = DateTime.Now.TimeOfDay;
            TimeEnd = TimeStart + TimeSpan.FromHours(4);
            BookSelectedRoomCommand = new RelayCommand(RoomHandler.CreateBooking, RoomHandler.RoomIsSelectedCheck);
            GoBackCommand = new RelayCommand(RoomHandler.GoBackMethod, null);
            SelectedRoomtypeFilter = RoomtypeList[0];
            SelectedBuildingFilter = BuildingList[0];
            RoomHandler.FilterSearchMethod();
        }
        #region Properties
        public RelayCommand BookSelectedRoomCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public string selectedLocation => LocationsVM.SelectedLocation.City;
        public ObservableCollection<RoomsView> RoomList { get; }
        public RoomHandler RoomHandler { get; set; }
        #endregion

        #region Properties That Changes In View
        public RoomsView SelectedRoomsView
        {
            get { return _selectedRoomsView; }
            set
            {
                _selectedRoomsView = value;
                OnPropertyChanged(nameof(RoomHandler.RoomIsSelectedCheck));
                BookSelectedRoomCommand.RaiseCanExecuteChanged();
            }
        }

        public TimeSpan TimeStart { 
            get { return _selectedStartTime; }
            set
            {
                _selectedStartTime = value;
                OnPropertyChanged(nameof(TimeStart));
                if (TimeEnd != TimeSpan.Zero && TimeStart != TimeSpan.Zero)
                {
                    RoomHandler.FilterSearchMethod();
                }
            }
        }

        public TimeSpan TimeEnd 
        { 
            get { return _selectedEndTime; }
            set
            {
                _selectedEndTime = value;
                OnPropertyChanged(nameof(TimeEnd));
                if (TimeEnd != TimeSpan.Zero && TimeStart != TimeSpan.Zero)
                {
                    RoomHandler.FilterSearchMethod();
                }
            }
        }

        public DateTimeOffset Date 
        { 
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
                if (TimeEnd != TimeSpan.Zero && TimeStart != TimeSpan.Zero)
                {
                    RoomHandler.FilterSearchMethod();
                }
            }
        }

        public string SelectedBuildingFilter 
        { 
            get { return _selectedBuilding; }
            set
            {
                _selectedBuilding = value;
                OnPropertyChanged();
                RoomHandler.FilterSearchMethod();
            }
        }
        public string SelectedRoomtypeFilter 
        { 
            get { return _selectedRoomType; }
            set
            {
                _selectedRoomType = value;
                OnPropertyChanged();
                RoomHandler.FilterSearchMethod();
            }
        }


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
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
