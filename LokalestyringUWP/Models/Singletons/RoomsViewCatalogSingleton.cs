using LokalestyringUWP.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models.Singletons
{
    public class RoomsViewCatalogSingleton
    {
        private static RoomsViewCatalogSingleton _instance = null;
        public ObservableCollection<RoomsView> RoomsView { get; }
        public static RoomsViewCatalogSingleton Instance { get { return _instance ?? (_instance = new RoomsViewCatalogSingleton()); } }
        public RoomsViewCatalogSingleton()
        {
            RoomsView = new ObservableCollection<RoomsView>();
            LoadRoomsViewAsync();
        }


        public async void LoadRoomsViewAsync()
        {
            ObservableCollection<RoomsView> roomsView = await PersistancyService.LoadTableFromJsonAsync<RoomsView>("RoomsViews");

            foreach (var item in roomsView)
            {
                this.RoomsView.Add(item);
            }
        }
    }
}
