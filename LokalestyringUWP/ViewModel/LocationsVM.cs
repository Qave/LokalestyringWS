using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using LokalestyringUWP.Models;
using LokalestyringUWP.Service;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LokalestyringUWP.Annotations;
using LokalestyringUWP.Common;
using LokalestyringUWP.Handler;
using LokalestyringUWP.View;

namespace LokalestyringUWP.ViewModel
{
    public class LocationsVM
    {
        public ObservableCollection<Location> Locations { get; set; }
        private static Location _selectedLocation { get; set; }

        public static Location SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                _selectedLocation = value;
                setLocation();
            }
        }

        public LocationsVM()
        {
            Locations = new ObservableCollection<Location>();
            Locations.Add(new Location() { Loc_Id = 1, City = "Roskilde", Name = "RO" });
            Locations.Add(new Location() { Loc_Id = 2, City = "Køge", Name = "KOE" });
            Locations.Add(new Location() { Loc_Id = 3, City = "Næstved", Name = "NAE" });

            //LoadLocations();
        }
        
        private static void setLocation()
        {
            ((Frame)Window.Current.Content).Navigate(typeof(PageBookRooms)); //redirect to the next page
        }
        
        public async void LoadLocations()
        {
            Locations = await PersistancyService.LoadLocationsFromJsonAsync();
        }
    }
}
