using LokalestyringUWP.Common;
using LokalestyringUWP.Handler;
using LokalestyringUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LokalestyringUWP.ViewModel
{
    class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            RoomHandler = new RoomHandler(this);
            RoomList = new ObservableCollection<Room>();
            //RoomList = RoomCatalogSingleton.Instance.Rooms;
            RoomList.Add(new Room(1, 1, "01", 1, 2, 1, "A","RO", "Klasselokale"));
            RoomList.Add(new Room(2, 1, "04", 1, 2, 1, "A","RO", "Klasselokale"));
            RoomList.Add(new Room(3, 1, "10", 1, 2, 1, "B","RO", "Lille"));
            RoomList.Add(new Room(4, 2, "22", 1, 2, 1, "B","RO", "Lille"));
            RoomList.Add(new Room(5, 2, "35", 1, 2, 1, "C","RO", "Medium"));
            RoomList.Add(new Room(6, 2, "05", 1, 2, 1, "C","RO", "Medium"));
            RoomList.Add(new Room(7, 3, "11", 1, 2, 1, "D","RO", "Klasselokale"));
            RoomList.Add(new Room(8, 3, "17", 1, 2, 1, "D","RO", "Klasselokale"));

            FilterSearchCommand = new RelayCommand(RoomHandler.FilterSearchMethod, null);
            OnPropertyChanged(nameof(RoomList));
        }

        public RoomHandler RoomHandler { get; set; }
        public ICommand FilterSearchCommand { get; set; }
        public DateTimeOffset Date { get; set; }
        public ObservableCollection<Room> RoomList { get; set; }

        public List<string> RoomtypeList
        {
            get
            {
                return new List<string>
                {
                    "Lille", "Medium", "Klasselokale"
                };
            }
        }

        public List<char> BuildingList
        {
            get
            {
                return new List<char>
                {
                    'A', 'B', 'C', 'D'
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
