using System.Collections.ObjectModel;
using LokalestyringUWP.Service;

namespace LokalestyringUWP.Models.Singletons
{
    class RoomCatalogSingleton
    {
        private static RoomCatalogSingleton _instance = null;

        public ObservableCollection<RoomsView> Rooms { get; }
        public static RoomCatalogSingleton Instance { get { return _instance ?? (_instance = new RoomCatalogSingleton()); } }

        public RoomCatalogSingleton()
        {
            Rooms = new ObservableCollection<RoomsView>();
            LoadRoomsAsync();
        }

        public async void LoadRoomsAsync()
        {
            ObservableCollection<RoomsView> rooms = await PersistancyService.LoadRoomsFromJsonAsync();

            foreach (var item in rooms)
            {
                this.Rooms.Add(item);
            }
        }

    }
}
