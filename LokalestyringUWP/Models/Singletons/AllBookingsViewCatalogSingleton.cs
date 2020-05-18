using LokalestyringUWP.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Models.Singletons
{
    public class AllBookingsViewCatalogSingleton : INotifyPropertyChanged
    {
        private static AllBookingsViewCatalogSingleton _instance = null;
        public ObservableCollection<AllBookingsView> AllBookings { get; }
        public static AllBookingsViewCatalogSingleton Instance { get { return _instance ?? (_instance = new AllBookingsViewCatalogSingleton()); } }
        public AllBookingsViewCatalogSingleton()
        {
            AllBookings = new ObservableCollection<AllBookingsView>();
            LoadAllBookingsAsync();
        }

        public async Task LoadAllBookingsAsync()
        {
            ObservableCollection<AllBookingsView> allBookings = await PersistancyService.LoadTableFromJsonAsync<AllBookingsView>("AllBookingsViews");

            foreach (var item in allBookings)
            {
                this.AllBookings.Add(item);
            }
        }

        #region INotifyPropertyChanged interface implementation
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Refreshes the property on the pageview.
        /// </summary>
        /// <param name="propertyName">You can specify the property to update when using "nameof(propertyName)" as a parameter</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
