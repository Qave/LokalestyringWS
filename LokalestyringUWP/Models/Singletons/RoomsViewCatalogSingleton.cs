using LokalestyringUWP.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models.Singletons
{
    public class RoomsViewCatalogSingleton
    {
        private static RoomsViewCatalogSingleton _instance = null;
        public ObservableCollection<RoomsView> RoomsViewCollection { get; }
        public static RoomsViewCatalogSingleton Instance { get { return _instance ?? (_instance = new RoomsViewCatalogSingleton()); } }
        public RoomsViewCatalogSingleton()
        {
            RoomsViewCollection = new ObservableCollection<RoomsView>();
            LoadbookingsAsync();
        }

        public async void LoadbookingsAsync()
        {
            ObservableCollection<RoomsView> roomsViewCollection = await PersistancyService.LoadTableFromJsonAsync<RoomsView>("RoomsViews");

            foreach (var item in roomsViewCollection)
            {
                this.RoomsViewCollection.Add(item);
            }
        }
    }
}
