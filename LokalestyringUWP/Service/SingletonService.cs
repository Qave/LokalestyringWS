using LokalestyringUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Service
{
    public class SingletonService<T>
    {
        
        public const string serverUrl = "http://localhost:51531";
        private static SingletonService<T> _instance = null;

        public ObservableCollection<ObservableCollection<T>> AllLists { get; }
        public ObservableCollection<Booking> Bookings { get; }
        public ObservableCollection<Location> Locations { get; }
        public ObservableCollection<Building> Buildings { get; }
        public ObservableCollection<Room> Rooms { get; }
        public ObservableCollection<Roomtype> Roomtypes { get; }
        public ObservableCollection<TavleBooking> TavleBookings { get; }
        public ObservableCollection<User> Users { get; }

        public static SingletonService<T> Instance { get { return _instance ?? (_instance = new SingletonService<T>()); } }

        public SingletonService()
        {
            Collection = new ObservableCollection<T>();
            LoadTableAsync();
        }
        /// <summary>
        /// VIRKER IKKE MED PERSON / PEOPLE KUN BOOKING / BOOOKINGS, BASICLY ALT DER KAN SMIDES ET "S" PÅ DER DEREFTER BLIVER PLURALIZED
        /// </summary>
        public async void LoadTableAsync()
        {
            ObservableCollection<T> Table = await PersistancyService.LoadTableFromJsonAsync<T>(typeof(T).Name+"s");

            foreach (var row in Table)
            {
                this.Collection.Add(row);
            }
        }
    }
}
