using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LokalestyringUWP.Service;

namespace LokalestyringUWP.Models.Singletons
{
    public class RoomtypeCatalogSingleton
    {
        public const string serverUrl = "http://localhost:51531";
        private static RoomtypeCatalogSingleton _instance = null;

        public ObservableCollection<Roomtype> Roomtypes { get; }
        public static RoomtypeCatalogSingleton Instance { get { return _instance ?? (_instance = new RoomtypeCatalogSingleton()); } }

        public RoomtypeCatalogSingleton()
        {
            Roomtypes = new ObservableCollection<Roomtype>();
            LoadRoomtypesAsync();
        }

        public async void LoadRoomtypesAsync()
        {
            ObservableCollection<Roomtype> roomtypes = await PersistancyService.LoadTableFromJsonAsync<Roomtype>("Roomtypes");

            foreach (var item in roomtypes)
            {
                this.Roomtypes.Add(item);
            }
        }
}
