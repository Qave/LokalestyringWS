using System.Collections.ObjectModel;
using LokalestyringUWP.Service;

namespace LokalestyringUWP.Models.Singletons
{
    public class BookingCatalogSingleton
    {
        public const string serverUrl = "http://localhost:51531";
        private static BookingCatalogSingleton _instance = null;

        public ObservableCollection<Booking> Bookings { get; }
        public static BookingCatalogSingleton Instance { get { return _instance ?? (_instance = new BookingCatalogSingleton()); } }

        public BookingCatalogSingleton()
        {
            Bookings = new ObservableCollection<Booking>();
            LoadRoomsAsync();
        }

        public async void LoadRoomsAsync()
        {
            ObservableCollection<Booking> rooms = await PersistancyService.LoadTableFromJsonAsync<Booking>("Bookings");

            foreach (var item in rooms)
            {
                this.Bookings.Add(item);
            }
        }
    }
}
