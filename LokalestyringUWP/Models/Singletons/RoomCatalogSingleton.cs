using System.Collections.ObjectModel;
using LokalestyringUWP.Service;

namespace LokalestyringUWP.Models.Singletons
{
    class RoomCatalogSingleton
    {
        public const string serverUrl = "http://localhost:51531";
        private static RoomCatalogSingleton _instance = null;

        public ObservableCollection<Room> Rooms { get; }
        public static RoomCatalogSingleton Instance { get { return _instance ?? (_instance = new RoomCatalogSingleton()); } }

        public RoomCatalogSingleton()
        {
            Rooms = new ObservableCollection<Room>();
            LoadRoomsAsync();
        }

        public async void LoadRoomsAsync()
        {
            ObservableCollection<Room> rooms = await PersistancyService.LoadRoomsFromJsonAsync();

            foreach (var item in rooms)
            {
                this.Rooms.Add(item);
            }
        }

    }
}
