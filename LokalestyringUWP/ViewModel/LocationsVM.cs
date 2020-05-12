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
        public ObservableCollection<Location> Locations { get; set; }
        public static Location selectedLocation { get; set; }

        public LocationsVM()
        {
            Locations = new ObservableCollection<Location>();
            Locations.Add(new Location() { Loc_Id = 1, City = "Roskilde", Name = "RO" });
            Locations.Add(new Location() { Loc_Id = 2, City = "Køge", Name = "KOE" });
            Locations.Add(new Location() { Loc_Id = 3, City = "Næstved", Name = "NAE" });


            //LoadLocations();
        }

        public async void LoadLocations()
        {
            Locations = await PersistancyService.LoadLocationsFromJsonAsync();
        }
    }
}
