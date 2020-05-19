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
        public TeacherControlPanelVM TCPREF { get; set; }
        public UserCatalogSingleton Usersingleton { get; set; }
        public string MailAddress { get; set; }

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
        //Easier to just steal the booking instead of deleting the booking and then having to go back and redo the whole booking
        /// <summary>
        /// This Method steals a booking if a series of conditions are met
        /// </summary>
        public void TeacherStealsBookingMethod()
        {
            var result = DialogHandler.GenericYesNoDialog($"Er du sikker på du vil booke dette rum? {TCPREF.BookingIsSelected.RoomName}, ved at booke dette lokale aflyser du en elves booking", "Er du sikker?", "Ja", "Nej").Result;
            if (result)
            {

                if (LoginHandler.SelectedUser.Teacher == true)
                {
                    if (!IsATeach())
                    {
                        if (TCPREF.Date.Date >= DateTime.Now.Date.AddDays(3))
                        {
                            GetMailFromUser();
                            MailService.MailSender(MailAddress, "En lærer aflyste din booking", $"Din booking den {TCPREF.BookingIsSelected.Date} fra {TCPREF.TimeStart} til {TCPREF.TimeEnd} i rum {TCPREF.BookingIsSelected.RoomName} er blevet aflyst {LoginHandler.SelectedUser.User_Name}, vi beklager ulejligheden, du er selvfølgelig velkommen til at booke et nyt rum op appen", true);
                            PersistancyService.UpdateAsJsonAsync(new Booking()
                            {
                                Date = TCPREF.Date.Date,
                                TavleBookings = null,
                                Time_start = TCPREF.TimeStart,
                                Time_end = TCPREF.TimeEnd,
                                User_Id = LoginHandler.CurrentUserId
                            }, "Bookings", TCPREF.BookingIsSelected.Booking_Id);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// This Method removes a booking if a series of conditions are met
        /// </summary>
        public void TeacherCancelsBookingMethod()
        {
            if (LoginHandler.SelectedUser.Teacher == true)
            {
                if (!IsATeach())
                {
                    if (TCPREF.Date.Date >= DateTime.Now.Date.AddDays(3))
                    {
                        GetMailFromUser();
                        MailService.MailSender(MailAddress, "En lærer aflyste din booking", $"Din booking den {TCPREF.BookingIsSelected.Date} fra {TCPREF.TimeStart} til {TCPREF.TimeEnd} i rum {TCPREF.BookingIsSelected.RoomName} er blevet aflyst {LoginHandler.SelectedUser.User_Name}, vi beklager ulejligheden, du er selvfølgelig velkommen til at booke et nyt rum op appen", true);
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

        public void GetMailFromUser()
        {
            foreach (var user in Usersingleton.Users)
            {
                if (user.User_Id == TCPREF.BookingIsSelected.User_Id)
                {
                    MailAddress = user.User_Email;
                    break;
                }
                throw new Exception("No Mail Found");
            }
        }
    }
}

