using LokalestyringUWP.Handler;
using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;
using LokalestyringUWP.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LokalestyringUWP.ViewModel
{
    public class UserBookingsVM : INotifyPropertyChanged
    {
        private UserBookingsView _selectedBooking;
        public ObservableCollection<UserBookingsView> UserBookingsViewCollectionFromSingleton { get; set; }
        public ObservableCollection<TavleBooking> Tavlebookings { get; set; }
        public UserBookingsVM()
        {
            UserBookingsViewCollectionFromSingleton = new ObservableCollection<UserBookingsView>();
            Tavlebookings = new ObservableCollection<TavleBooking>();

            UserBookingsViewCollectionFromSingleton = UserBookingsCatalogSingleton.Instance.UserBookings;
            Tavlebookings = TavleBookingCatalogSingleton.Instance.TavleBookings;
            UserBookingsOnId(1);
        }
        public UserBookingsView SelectedBooking { get { return _selectedBooking; } set { _selectedBooking = value; OnPropertyChanged(); } }
        



        public void UserBookingsOnId(int userid)
        {
            var query = (from c in UserBookingsViewCollectionFromSingleton
                        select c).Where(c => c.User_Id == userid).ToList();

            UserBookingsViewCollectionFromSingleton.Clear();
            foreach (var item in query)
            {
                UserBookingsViewCollectionFromSingleton.Add(item);
            }
        }


        public string CheckIfTavleBookingExists()
        {
            //var query = from t in Tavlebookings
            //            where t.Booking_Id == SelectedBooking.boo
            return "";
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
