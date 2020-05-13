using LokalestyringUWP.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models.Singletons
{
    public class LocationSingleton
    {
        private static LocationSingleton _instance = null;
        public ObservableCollection<Location> Locations { get; }
        public static LocationSingleton Instance { get { return _instance ?? (_instance = new LocationSingleton()); } }
        public LocationSingleton()
        {
            Locations = new ObservableCollection<Location>();
            LoadLocationsAsync();
        }

        public async void LoadLocationsAsync()
        {
            ObservableCollection<Location> locations = await PersistancyService.LoadTableFromJsonAsync<Location>("Locations");

            foreach (var item in locations)
            {
                this.Locations.Add(item);
            }
        }
    }
}
