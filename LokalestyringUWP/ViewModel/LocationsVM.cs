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
using LokalestyringUWP.Models.Singletons;
using LokalestyringUWP.View;

namespace LokalestyringUWP.ViewModel
{
    public class LocationsVM
    {
        /// <summary>
        /// These properties are for binding in the UWP project
        /// </summary>
        public ObservableCollection<Location> Locations
        {
            get { return LocationSingleton.Instance.Locations; }
        }
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
        public User CurrentUser { get {return LoginHandler.SelectedUser; } }

        public LocationsVM()
        {
            
        }
        /// <summary>
        /// Checks if selectedlocation is null if it's not null it changes the page to PageBookRooms.
        /// </summary>
        private static void setLocation()
        {
            if (SelectedLocation != null)
            {
                ((Frame)Window.Current.Content).Navigate(typeof(PageBookRooms)); //redirect to the next page
            }
        }
    }
}
