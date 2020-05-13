using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;
using LokalestyringUWP.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.ViewModel
{
    public class UserBookingsVM
    {
        
        public ObservableCollection<RoomsView> Bookings { get; set; }
        public ObservableCollection<Location> Locations { get; set; }
        public ObservableCollection<Location> Buildings { get; set; }

        public UserBookingsVM()
        {
            Bookings = new ObservableCollection<RoomsView>();
            Bookings = RoomsViewCatalogSingleton.Instance.RoomsViewCollection;

        }
    }
}
