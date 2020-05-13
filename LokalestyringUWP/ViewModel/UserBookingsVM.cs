using LokalestyringUWP.Handler;
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
        
        public ObservableCollection<UserBookingsView> UserBookingsViewCollection { get; set; }
        public UserBookingsVM()
        {
            UserBookingsViewCollection = new ObservableCollection<UserBookingsView>();
            UserBookingsViewCollection = UserBookingsCatalogSingleton.Instance.UserBookings;

        }
        public void UserBookingsOnId(int userid)
        {
            //var query = from c in UserBookingsViewCollection
            //            where c.User_Id = LoginHandler.CurrentUserId
            //            select c;
        }
    }
}
