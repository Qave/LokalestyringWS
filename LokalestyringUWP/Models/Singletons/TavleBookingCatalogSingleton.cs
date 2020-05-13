using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LokalestyringUWP.Service;

namespace LokalestyringUWP.Models.Singletons
{
    public class TavleBookingCatalogSingleton
    {
        public const string serverUrl = "http://localhost:51531";
        private static TavleBookingCatalogSingleton _instance = null;

        public ObservableCollection<TavleBooking> TavleBookings { get; }
        public static TavleBookingCatalogSingleton Instance { get { return _instance ?? (_instance = new TavleBookingCatalogSingleton()); } }

        public TavleBookingCatalogSingleton()
        {
            TavleBookings = new ObservableCollection<TavleBooking>();
            LoadTavleBookingsAsync();
        }

        public async void LoadTavleBookingsAsync()
        {
            ObservableCollection<TavleBooking> tavleBookings = await PersistancyService.LoadTableFromJsonAsync<TavleBooking>("TavleBookings");

            foreach (var item in tavleBookings)
            {
                this.TavleBookings.Add(item);
            }
        }
    }
}
