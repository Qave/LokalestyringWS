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
using Windows.UI.Xaml;

namespace LokalestyringUWP.ViewModel
{
    public class UserBookingsVM : INotifyPropertyChanged
    {
        private AllBookingsView _selectedBooking;
        public ObservableCollection<AllBookingsView> AllBookingsViewSingleton { get; set; }
        public ObservableCollection<TavleBooking> Tavlebookings { get; set; }
        public UserBookingsVM()
        {
            AllBookingsViewSingleton = new ObservableCollection<AllBookingsView>();
            Tavlebookings = new ObservableCollection<TavleBooking>();

            AllBookingsViewSingleton = AllBookingsViewCatalogSingleton.Instance.AllBookings;
            Tavlebookings = TavleBookingCatalogSingleton.Instance.TavleBookings;
            BookTavleBtnVisibility = Visibility.Collapsed;
            UserBookingsOnId(1);
        }
        public AllBookingsView SelectedBooking 
        { 
            get 
            { return _selectedBooking; } 
            set
            {
                _selectedBooking = value;
                CheckIfTavleBookingExists();
                OnPropertyChanged();
                OnPropertyChanged(nameof(BookTavleBtnVisibility));
            } 
        }
        



        public void UserBookingsOnId(int userid)
        {
            var query = (from c in AllBookingsViewSingleton
                         select c).Where(c => c.User_Id == userid).ToList();

            AllBookingsViewSingleton.Clear();
            foreach (var item in query)
            {
                AllBookingsViewSingleton.Add(item);
            }
        }
        public Visibility BookTavleBtnVisibility
        {
            get;set;
        }
        public Visibility CheckIfTavleBookingExists()
        {
            if (SelectedBooking == null)
            {
                return BookTavleBtnVisibility = Visibility.Collapsed;
            }
            else
            {
                var query = (from t in Tavlebookings
                             select t).Where(x => x.Booking_Id == SelectedBooking.Booking_Id).ToList();
                if (query.Count < 1)
                {
                    return BookTavleBtnVisibility = Visibility.Collapsed;
                }
                else
                {
                    return BookTavleBtnVisibility = Visibility.Visible;
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
