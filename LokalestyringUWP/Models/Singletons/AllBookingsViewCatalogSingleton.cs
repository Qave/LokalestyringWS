using LokalestyringUWP.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models.Singletons
{
    public class AllBookingsViewCatalogSingleton
    {
        private static AllBookingsViewCatalogSingleton _instance = null;
        public ObservableCollection<AllBookingsView> AllBookings { get; }
        public static AllBookingsViewCatalogSingleton Instance { get { return _instance ?? (_instance = new AllBookingsViewCatalogSingleton()); } }
        public AllBookingsViewCatalogSingleton()
        {
            AllBookings = new ObservableCollection<AllBookingsView>();
            LoadAllBookingsAsync();
        }

        public async void LoadAllBookingsAsync()
        {
            ObservableCollection<AllBookingsView> allBookings = await PersistancyService.LoadTableFromJsonAsync<AllBookingsView>("AllBookingsViews");

            foreach (var item in allBookings)
            {
                this.AllBookings.Add(item);
            }
        }
    }
}
