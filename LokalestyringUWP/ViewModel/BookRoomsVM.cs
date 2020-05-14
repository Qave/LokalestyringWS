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
    class BookRoomsVM : INotifyPropertyChanged
    {
        public BookRoomsVM()
        {
            RoomHandler = new RoomHandler(this);
            RoomList = new ObservableCollection<RoomsView>();
            ResettedList = new ObservableCollection<RoomsView>();
            FilterSearchCommand = new RelayCommand(RoomHandler.FilterSearchMethod, null);
            CommandHandler.GoBackCommand = new RelayCommand(RoomHandler.GoBackMethod, null);
            OnPropertyChanged(nameof(RoomList));
            SelectedRoomtypeFilter = RoomtypeList[0];
            Date = DateTimeOffset.Now.ToUniversalTime();
            SelectedBuildingFilter = BuildingList[0];
            RoomHandler.RestoreList();
        }
        public ObservableCollection<RoomsView> ResettedList { get; set; }

        public RoomHandler RoomHandler { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public DateTimeOffset Date { get; set; }
        public string SelectedBuildingFilter { get; set; }
        public string SelectedRoomtypeFilter { get; set; }
        public ICommand FilterSearchCommand { get; set; }
        public ObservableCollection<RoomsView> RoomList { get; set; }

        public string selectedLocation => LocationsVM.SelectedLocation.City;
        public ICommand GoBackCommand => CommandHandler.GoBackCommand;

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
