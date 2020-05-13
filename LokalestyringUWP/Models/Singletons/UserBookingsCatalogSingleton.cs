using LokalestyringUWP.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models.Singletons
{
    public class UserBookingsCatalogSingleton
    {
        private static UserBookingsCatalogSingleton _instance = null;
        public ObservableCollection<UserBookingsView> UserBookings { get; }
        public static UserBookingsCatalogSingleton Instance { get { return _instance ?? (_instance = new UserBookingsCatalogSingleton()); } }
        public UserBookingsCatalogSingleton()
        {
            UserBookings = new ObservableCollection<UserBookingsView>();
            LoadUserBookingsAsync();
        }

        public async void LoadUserBookingsAsync()
        {
            ObservableCollection<UserBookingsView> userBookings = await PersistancyService.LoadTableFromJsonAsync<UserBookingsView>("UserBookingsViews");

            foreach (var item in userBookings)
            {
                this.UserBookings.Add(item);
            }
        }
    }
}
