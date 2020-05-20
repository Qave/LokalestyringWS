using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using Windows.UI.Input;
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
        /// <summary>
        /// A reference to TeacherControlPanel
        /// </summary>
        public TeacherControlPanelVM TCPREF { get; set; }
        
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
        /// This method clears the BookingList, which is a list of AllBookings and refills it with all the classrooms from the AllBookingsView singleton
        /// </summary>
        public void ResetList()
        {
            TCPREF.BookingList.Clear();
            var query = (from r in AllBookingsViewCatalogSingleton.Instance.AllBookings
                         where r.Type == "Klasselokale"
                         select r).ToList();

            foreach (var item in query)
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
        public async Task TeacherStealsBookingMethod()
        {
            var result = await DialogHandler.GenericYesNoDialog(
                $"Er du sikker på du vil booke dette rum? {TCPREF.BookingIsSelected.RoomName}, ved at booke dette lokale aflyser du en elves booking",
                "Er du sikker?", "Ja", "Nej");
            if (result)
            {
                if (LoginHandler.SelectedUser.Teacher == true)
                {
                    if (IsNotATeach())
                    {
                        if (TCPREF.BookingIsSelected.Date.Date >= DateTime.Now.Date.AddDays(3))
                        {
                            TeacherSnatchRoom();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This Method removes a booking if a series of conditions are met
        /// </summary>
        public async Task TeacherCancelsBookingMethod()
        {
            if (LoginHandler.SelectedUser.Teacher == true)
            {
                if (IsNotATeach())
                {
                    if (TCPREF.InputDate.Date >= DateTime.Now.Date.AddDays(3))
                    {
                        //await GetMailToUser("En lærer aflyste din booking", $"Din booking den {TCPREF.BookingIsSelected.Date.ToString("dd/MM/yyyy")} fra {TCPREF.BookingIsSelected.BookingStart} til {TCPREF.BookingIsSelected.BookingEnd} i rum {TCPREF.BookingIsSelected.RoomName} er blevet aflyst {LoginHandler.SelectedUser.User_Name}, vi beklager ulejligheden, du er selvfølgelig velkommen til at booke et nyt rum op appen", true);
                        PersistancyService.DeleteFromDatabaseAsync("Bookings", TCPREF.BookingIsSelected.Booking_Id);
                    }
                }
            }
        }

        /// <summary>
        /// Bool method that decides if a booking is not a teacher booking
        /// </summary>
        /// <returns></returns>
        public bool IsNotATeach()
        {

            //skal tjekke hele den nye bookings tidsintervaller for om der er en lærer der har booket på samme tid 

            //var query = (from b in BookingSingleton.Bookings
            //where b.Room_Id == TCPREF.BookingIsSelected.Room_Id && TCPREF.InputDate.Date == TCPREF.BookingIsSelected.Date.Date && b.Time_end >= TCPREF.InuputTimeStart && b.Time_start <= TCPREF.InputTimeEnd
            //select b).ToList();
            var query = from t in UserCatalogSingleton.Instance.Users
                join b in BookingCatalogSingleton.Instance.Bookings on t.User_Id equals b.User_Id
                        where b.User_Id == t.User_Id
                        select t;
            if (query.Any(l => l.User_Id == TCPREF.BookingIsSelected.User_Id))
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Method to find a mail connected to the booking of a user and sends them a message
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="subject">Subject eg. "Cancel"</param>
        /// <param name="body">The message you want to send</param>
        /// <param name="HTML">True if the body contains HTML for styling</param>
        /// <returns></returns>
        public async Task GetMailToUser(int userId, string subject, string body, bool HTML)
        {
            bool found = false;
            foreach (var user in UserCatalogSingleton.Instance.Users)
            {
                if (user.User_Id == userId)
                {
                    found = true;
                    MailAddress = user.User_Email;
                    break;
                }
            }
            if (found)
            {
                await MailService.MailSender(MailAddress, subject, body, HTML);
            }
            if (!found)
            {
                throw new Exception("No Email found");
            }
        }
        /// <summary>
        /// This method replaces every booking on a room in between the selected timeend and timestart
        /// </summary>
        /// <returns></returns>
        public async Task TeacherSnatchRoom()
        {
            var query = (from b in BookingCatalogSingleton.Instance.Bookings
                where b.Room_Id == TCPREF.BookingIsSelected.Room_Id && TCPREF.InputDate.Date == TCPREF.BookingIsSelected.Date.Date && b.Time_end >= TCPREF.InuputTimeStart && b.Time_start <= TCPREF.InputTimeEnd
                select b).ToList();

            foreach (var item in query)
            {
                PersistancyService.DeleteFromDatabaseAsync("Bookings",item.Booking_Id);
                await GetMailToUser(item.User_Id,"En lærer aflyste din booking", $"Din booking den {TCPREF.BookingIsSelected.Date.ToString("dd/MM/yyyy")} fra {new DateTime(TCPREF.BookingIsSelected.BookingStart.Ticks).ToString("HH:mm")} til {new DateTime(TCPREF.BookingIsSelected.BookingEnd.Ticks).ToString("HH:mm")} i rum {TCPREF.BookingIsSelected.RoomName} er blevet aflyst af {LoginHandler.SelectedUser.User_Name}, vi beklager ulejligheden, du er selvfølgelig velkommen til at booke et nyt rum på appen", true);
            }

            await PersistancyService.SaveInsertAsJsonAsync(new Booking
            {
                Date = TCPREF.InputDate.Date,
                Room_Id = TCPREF.BookingIsSelected.Room_Id,
                TavleBookings = null,
                Time_start = TCPREF.InuputTimeStart,
                Time_end = TCPREF.InputTimeEnd,
                User_Id = LoginHandler.CurrentUserId
            },"Bookings");
            ResetList();
        }
    }
}

