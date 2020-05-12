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

namespace LokalestyringUWP.ViewModel
{
    class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            RoomList = new ObservableCollection<Room>();
            //RoomList = RoomCatalogSingleton.Instance.Rooms;
            RoomList.Add(new Room(1, 1, "01", 1, 2, 1, "Bygning A","RO", "Klasselokale"));
            RoomList.Add(new Room(1, 1, "01", 1, 2, 1, "Bygning A","RO", "Klasselokale"));
            RoomList.Add(new Room(1, 1, "01", 1, 2, 1, "Bygning A","RO", "Klasselokale"));
            RoomList.Add(new Room(1, 1, "01", 1, 2, 1, "Bygning A","RO", "Klasselokale"));
            RoomList.Add(new Room(1, 1, "01", 1, 2, 1, "Bygning A","RO", "Klasselokale"));
            RoomList.Add(new Room(1, 1, "01", 1, 2, 1, "Bygning A","RO", "Klasselokale"));
            RoomList.Add(new Room(1, 1, "01", 1, 2, 1, "Bygning A","RO", "Klasselokale"));
            RoomList.Add(new Room(1, 1, "01", 1, 2, 1, "Bygning A","RO", "Klasselokale"));
            OnPropertyChanged(nameof(RoomList));
        }

        public ObservableCollection<Room> RoomList { get; set; }

        public List<string> RoomTypes
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


        //public static void AddRoomDummyData()
        //{
        //    RoomCatalogSingleton.Instance.Rooms.Add(new Room(1, 1, "fisk", 1, 2, 1, "RO", "Klasse", "Aids"));
        //    RoomCatalogSingleton.Instance.Rooms.Add(new Room(1, 1, "fisk", 1, 2, 1, "RO", "Klasse", "Aids"));
        //    RoomCatalogSingleton.Instance.Rooms.Add(new Room(1, 1, "fisk", 1, 2, 1, "RO", "Klasse", "Aids"));
        //    RoomCatalogSingleton.Instance.Rooms.Add(new Room(1, 1, "fisk", 1, 2, 1, "RO", "Klasse", "Aids"));
        //    RoomCatalogSingleton.Instance.Rooms.Add(new Room(1, 1, "fisk", 1, 2, 1, "RO", "Klasse", "Aids"));
        //    RoomCatalogSingleton.Instance.Rooms.Add(new Room(1, 1, "fisk", 1, 2, 1, "RO", "Klasse", "Aids"));
        //    RoomCatalogSingleton.Instance.Rooms.Add(new Room(1, 1, "fisk", 1, 2, 1, "RO", "Klasse", "Aids"));
        //    RoomCatalogSingleton.Instance.Rooms.Add(new Room(1, 1, "fisk", 1, 2, 1, "RO", "Klasse", "Aids"));
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
