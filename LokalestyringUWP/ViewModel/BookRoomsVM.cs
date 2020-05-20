﻿using LokalestyringUWP.Common;
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
        private RoomsView _selectedRoomsView;

        public RelayCommand BookSelectedRoomCommand { get; set; }
        public ICommand FilterSearchCommand => CommandHandler.FilterSearchCommand;
        public ICommand GoBackCommand => CommandHandler.GoBackCommand;
        public string selectedLocation => LocationsVM.SelectedLocation.City;

        public BookRoomsVM()
        {
            RoomHandler = new RoomHandler(this); //SKAL INITIALISERES FØRST
            Date = DateTimeOffset.Now.Date;
            TimeStart = DateTime.Now.TimeOfDay;
            TimeEnd = TimeStart + TimeSpan.FromHours(4);
            BookSelectedRoomCommand = new RelayCommand(RoomHandler.CreateBooking, RoomIsSelectedCheck);
            CommandHandler.BookSelectedRoomCommand = new RelayCommand(DialogHandler.ConfirmBookingDialog, null);
            CommandHandler.FilterSearchCommand = new RelayCommand(RoomHandler.FilterSearchMethod, null);
            CommandHandler.GoBackCommand = new RelayCommand(RoomHandler.GoBackMethod, null);
            RoomList = new ObservableCollection<RoomsView>();
            SelectedRoomtypeFilter = RoomtypeList[0];
            SelectedBuildingFilter = BuildingList[0];
            RoomHandler.FilterSearchMethod();

        }

        public ObservableCollection<RoomsView> RoomList { get; set; }

        public RoomHandler RoomHandler { get; set; }
        public RoomsView SelectedRoomsView
        {
            get { return _selectedRoomsView; }
            set
            {
                _selectedRoomsView = value;
                OnPropertyChanged(nameof(RoomIsSelectedCheck));
                BookSelectedRoomCommand.RaiseCanExecuteChanged();
            }
        }

        public bool RoomIsSelectedCheck()
        {
            return (SelectedRoomsView != null);
        }

        public TimeSpan TimeStart { get; set; }

        public TimeSpan TimeEnd { get; set; }

        public DateTimeOffset Date { get; set; }

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
