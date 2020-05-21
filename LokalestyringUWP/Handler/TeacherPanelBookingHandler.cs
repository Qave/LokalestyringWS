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
        #region Property and Reference
        /// <summary>
        /// A reference to TeacherControlPanel
        /// </summary>
        public TeacherControlPanelVM TCPREF { get; set; }
        /// <summary>
        /// A property for saving a mailaddress
        /// </summary>
        public string MailAddress { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// The Constructor of the class TeacherPanelBookingHandler
        /// </summary>
        /// <param name="rf"></param>
        public TeacherPanelBookingHandler(TeacherControlPanelVM rf)
        {
            TCPREF = rf;

        }
        #endregion

        #region Methods
        #region List Resetting Methods
        /// <summary>
        /// This method filters the listview with the correct objects
        /// </summary>
        public void FilterMethod()
        {
            if (TCPREF.InputTimeStart >= TCPREF.InputTimeEnd)
            {
                DialogHandler.Dialog("Vælg venligst en gyldig start- og sluttid.", "Ugyldigt tidspunkt");
            }
            else if (TCPREF.InputDate.Date < DateTime.Now.Date)
            {
                DialogHandler.Dialog("Vælg venligst en gyldig dato fra denne måned eller frem", "Ugyldig dato");
            }
            else
            {
                ResetList();
            }
        }

        /// <summary>
        /// This method clears the BookingList, which is a list of AllBookings and refills it with all the classrooms from the AllBookingsView singleton where the Date of the booking matches the selected date
        /// </summary>
        public async void ResetList()
        {
            AllBookingsViewCatalogSingleton.Instance.AllBookings.Clear();
            await AllBookingsViewCatalogSingleton.Instance.LoadAllBookingsAsync();
            TCPREF.BookingList.Clear();
            var query = (from r in AllBookingsViewCatalogSingleton.Instance.AllBookings
                         join l in RoomsViewCatalogSingleton.Instance.RoomsView on r.Room_Id equals l.Room_Id
                         where r.Type == "Klasselokale" && l.City == LocationsVM.SelectedLocation.City && r.Date.Date == TCPREF.InputDate.Date 
                         select r).ToList();

            foreach (var item in query)
            {
                TCPREF.BookingList.Add(item);
            }
            RemoveTeacherBooking();
        }
        #endregion

        #region Main Method

        /// <summary>
        /// This Method calls TeacherSnatchRoom if a series of conditions are met
        /// </summary>
        public async Task TeacherStealsBookingMethod()
        {
            var result = await DialogHandler.GenericYesNoDialog(
                $"Er du sikker på du vil booke dette lokale? {TCPREF.BookingIsSelected.RoomName}, ved at booke dette lokale aflyser du en eleves booking",
                "Er du sikker?", "Ja", "Nej");
            if (result)
            {
                if (LoginHandler.SelectedUser.Teacher)
                {
                    if (IsNotATeach())
                    {
                        if (TCPREF.BookingIsSelected.Date.Date >= DateTime.Now.Date.AddDays(3))
                        {
                            if (ThreeRoomsBookingLimit())
                            {
                                TeacherSnatchRoom();
                            }
                            else
                            {
                                DialogHandler.Dialog(
                                    "Du kan ikke have mere end tre bookinger af gangen, hvis du vil booke dette rum må du slette en anden booking",
                                    "Kun tre bookinger");
                            }
                        }
                        else
                        {
                            DialogHandler.Dialog("Der skal være minimum tre dages varsel før du kan booke et lokale",
                                "3-dages varsel-fejl!");
                        }
                    }
                    else
                    {
                        DialogHandler.Dialog("Den valgte booking indeholder en lærer-booking, det er desværre ikke muligt at slette en anden lærers booking", "lærer-booking fejl");
                    }
                }
            }
        }

        #endregion

        #region Teacher-Check Method

        public void RemoveTeacherBooking()
        {
            var query = (from b in AllBookingsViewCatalogSingleton.Instance.AllBookings
                join t in UserCatalogSingleton.Instance.Users on b.User_Id equals t.User_Id
                where b.User_Id == t.User_Id && t.Teacher
                select b).ToList();

            foreach (var item in query)
            {
                TCPREF.BookingList.Remove(item);
            }
        }
        /// <summary>
        /// Bool method that decides if a booking within the specified timeinterval contains a teacher booking Returns false if it doesn't
        /// </summary>
        /// <returns></returns>
        public bool IsNotATeach()
        {
            var query = from t in UserCatalogSingleton.Instance.Users
                        join b in AllBookingsViewCatalogSingleton.Instance.AllBookings on t.User_Id equals b.User_Id
                        where b.Room_Id == TCPREF.BookingIsSelected.Room_Id && TCPREF.InputDate.Date == TCPREF.BookingIsSelected.Date.Date && b.BookingEnd <= TCPREF.InputTimeStart && b.BookingStart >= TCPREF.InputTimeEnd && t.Teacher
                        select t;
            if (query.Any(l => l.Teacher))
            {

                return false;
            }
            return true;
        }
        /// <summary>
        /// A method making sure a teacher has no more than three booking on a inputted timeinterval
        /// </summary>
        /// <returns>Returns true if a teacher can book another room, and false if he has more than three booking</returns>
        public bool ThreeRoomsBookingLimit()
        {
            var query = (from b in AllBookingsViewCatalogSingleton.Instance.AllBookings
                         where b.User_Id == LoginHandler.CurrentUserId &&
                               b.Date.Date == TCPREF.BookingIsSelected.Date.Date
                               && b.BookingEnd >= TCPREF.InputTimeStart && b.BookingStart <= TCPREF.InputTimeEnd
                         group b by b.User_Id
                into RoomGroup
                         select new
                         {
                             LimitKey = RoomGroup.Key,
                             Count = RoomGroup.Count()
                         }).ToList();



            //var query = (from b in BookingCatalogSingleton.Instance.Bookings
            //             where b.User_Id == LoginHandler.CurrentUserId &&
            //                   b.Date.Date == TCPREF.BookingIsSelected.Date.Date
            //                  && b.Time_end >= TCPREF.InputTimeStart && b.Time_start <= TCPREF.InputTimeEnd
            //             group b by b.User_Id
            //    into RoomGroup
            //             select new
            //             {
            //                 LimitKey = RoomGroup.Key,
            //                 Count = RoomGroup.Count()
            //             }).ToList();

            foreach (var item in query)
            {
                if (item.Count >= 3)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Mail-Service Method
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
        #endregion

        #region Booking-Replace Method
        /// <summary>
        /// This method replaces every booking on a room in between the selected timeend and timestart
        /// </summary>
        /// <returns></returns>
        public async Task TeacherSnatchRoom()
        {
            var query = (from b in BookingCatalogSingleton.Instance.Bookings
                         where b.Room_Id == TCPREF.BookingIsSelected.Room_Id && TCPREF.BookingIsSelected.Date.Date == b.Date.Date && b.Time_end >= TCPREF.InputTimeStart && b.Time_start <= TCPREF.InputTimeEnd
                         select b).ToList();

            if (query.Count > 0)
            {
                foreach (var item in query)
                {
                    PersistancyService.DeleteFromDatabaseAsync("Bookings", item.Booking_Id);

                    await GetMailToUser(item.User_Id, "En lærer aflyste din booking", $"Din booking den " + $"{TCPREF.BookingIsSelected.Date.ToString("dd/MM/yyyy")} fra " + $"{new DateTime(TCPREF.BookingIsSelected.BookingStart.Ticks).ToString("HH:mm")} til " + $"{new DateTime(TCPREF.BookingIsSelected.BookingEnd.Ticks).ToString("HH:mm")} i rum {TCPREF.BookingIsSelected.RoomName} " + $"er blevet aflyst af {LoginHandler.SelectedUser.User_Name}, vi beklager ulejligheden. " + $"Du er selvfølgelig velkommen til at booke et nyt rum i appen.", true);
                }
                await PersistancyService.SaveInsertAsJsonAsync(new Booking
                {
                    Date = TCPREF.BookingIsSelected.Date.Date,
                    Room_Id = TCPREF.BookingIsSelected.Room_Id,
                    TavleBookings = null,
                    Time_start = new TimeSpan(TCPREF.InputTimeStart.Hours, TCPREF.InputTimeStart.Minutes, 0),
                    Time_end = new TimeSpan(TCPREF.InputTimeEnd.Hours, TCPREF.InputTimeEnd.Minutes, 0),
                    User_Id = LoginHandler.CurrentUserId
                }, "Bookings");
                MailService.MailSender(LoginHandler.SelectedUser.User_Email, "Kvittering på booking", $"Du har booket {TCPREF.BookingIsSelected.RoomName} " + $"d. {TCPREF.BookingIsSelected.Date.ToString("dd/MM/yyyy")} " + $"mellem {new DateTime(TCPREF.BookingIsSelected.BookingStart.Ticks).ToString("HH:mm")} og {new DateTime(TCPREF.BookingIsSelected.BookingEnd.Ticks).ToString("HH:mm")}.", true);
                ResetList();
                DialogHandler.Dialog("Din booking er nu oprettet. God dag!", "Booking Oprettet!");
            }

            if (query.Count <= 0)
            {
                var result = await DialogHandler.GenericYesNoDialog(
                    "Der er ikke nogen booking på dit valgte tidspunkt, er du sikker på at du valgte det rigtige tidspunkt?",
                    "Ingen booking på valgte tidspunkt", "Opret Ny booking i stedet", "Indtast tid igen");
                if (result)
                {
                    await PersistancyService.SaveInsertAsJsonAsync(new Booking
                    {
                        Date = TCPREF.BookingIsSelected.Date.Date,
                        Room_Id = TCPREF.BookingIsSelected.Room_Id,
                        TavleBookings = null,
                        Time_start = new TimeSpan(TCPREF.InputTimeStart.Hours, TCPREF.InputTimeStart.Minutes, 0),
                        Time_end = new TimeSpan(TCPREF.InputTimeEnd.Hours, TCPREF.InputTimeEnd.Minutes, 0),
                        User_Id = LoginHandler.CurrentUserId
                    }, "Bookings");
                    MailService.MailSender(LoginHandler.SelectedUser.User_Email, "Kvittering på booking", $"Du har booket {TCPREF.BookingIsSelected.RoomName} " + $"d. {TCPREF.BookingIsSelected.Date.ToString("dd/MM/yyyy")} " + $"mellem {new DateTime(TCPREF.BookingIsSelected.BookingStart.Ticks).ToString("HH:mm")} og {new DateTime(TCPREF.BookingIsSelected.BookingEnd.Ticks).ToString("HH:mm")}.", true);
                    ResetList();
                    DialogHandler.Dialog("Din booking er nu oprettet. God dag!", "Booking Oprettet!");
                }

            }
        }
        #endregion 
        #endregion
    }
}

