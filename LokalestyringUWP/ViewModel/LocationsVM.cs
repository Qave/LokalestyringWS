using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using LokalestyringUWP.Models;
using LokalestyringUWP.Service;

namespace LokalestyringUWP.ViewModel
{
    public class LocationsVM
    {
        ObservableCollection<Location> Locations { get; set; }
        static Location selectedLocation { get; set; }

        public LocationsVM()
        {
            Locations = new ObservableCollection<Location>();
            LoadLocations();
        }

        public async void LoadLocations()
        {
            Locations = await PersistancyService.LoadLocationsFromJsonAsync();
        }
    }
}
