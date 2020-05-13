using System.Collections.ObjectModel;
using LokalestyringUWP.Service;

namespace LokalestyringUWP.Models.Singletons
{
    public class BookingCatalogSingleton
    {
        private static BookingCatalogSingleton _instance = null;
        public ObservableCollection<Booking> Bookings { get; }
        public static BookingCatalogSingleton Instance { get { return _instance ?? (_instance = new BookingCatalogSingleton()); } }
        public BookingCatalogSingleton()
        {
            Bookings = new ObservableCollection<Booking>();
            LoadbookingsAsync();
        }

        public async void LoadbookingsAsync()
        {
            ObservableCollection<Booking> bookings = await PersistancyService.LoadTableFromJsonAsync<Booking>("Bookings");

            foreach (var item in bookings)
            {
                this.Bookings.Add(item);
            }
        }
    }
}
