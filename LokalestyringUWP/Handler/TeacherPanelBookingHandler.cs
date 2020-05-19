using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;
using LokalestyringUWP.Service;
using LokalestyringUWP.View;
using LokalestyringUWP.ViewModel;

namespace LokalestyringUWP.Handler
{
    /// <summary>
    /// This Class controls the Teacher's Booking-Override privileges 
    /// </summary>
    public class TeacherPanelBookingHandler
    {
        public UserBookingsVM UserBookingsRef;
        public TeacherControlPanelVM TCPREF { get; set; }
        public UserCatalogSingleton Usersingleton { get; set; }

        /// <summary>
        /// The Constructor of the class TeacherPanelBookingHandler
        /// </summary>
        /// <param name="rf"></param>
        public TeacherPanelBookingHandler(TeacherControlPanelVM rf)
        {
            TCPREF = rf;
        }


        /// <summary>
        /// This method clears the BookingList, which is a list of AllBookings and refills it from the AllBookingsView singleton
        /// </summary>
        public void ResetList()
        {
            TCPREF.BookingList.Clear();
            foreach (var item in AllBookingsViewCatalogSingleton.Instance.AllBookings)
            {
                TCPREF.BookingList.Add(item);
            }
        }

        /// <summary>
        /// This method navigates to the TeacherControlPanel
        /// </summary>
        public void TeacherControlPanelRedirect()
        {
            ((Frame)Window.Current.Content).Navigate(typeof(PageUserBookings));
        }
        /// <summary>
        /// This Method removes a booking if a series of conditions are met
        /// </summary>
        public void TeacherCancelBookingMethod()
        {
            if (LoginHandler.SelectedUser.Teacher == true)
            {
                if (!IsATeach())
                {
                    if (UserBookingsRef.SelectedBooking.Date.Date >= DateTime.Now.Date.AddDays(3))
                    {
                        PersistancyService.DeleteFromDatabaseAsync("Bookings", TCPREF.BookingIsSelected.Booking_Id);
                    }
                }
            }
        }
        /// <summary>
        /// Bool operator that decides if a booking is a teacher booking
        /// </summary>
        /// <returns></returns>
        public bool IsATeach()
        {
            var query = from t in Usersingleton.Users
                        where t.Teacher == true
                        select t;
            if (query.Any(l => l.User_Id == TCPREF.BookingIsSelected.User_Id))
            {
                return true;
            }
            return false;
        }
    }
}

