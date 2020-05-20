﻿using System;
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
        public void ResetList()
        {
            StartingList();
        }

        public async Task StartingList()
        {
            AllBookingsViewCatalogSingleton.Instance.AllBookings.Clear();
            await AllBookingsViewCatalogSingleton.Instance.LoadAllBookingsAsync();
            TCPREF.BookingList.Clear();
            var query = (from r in AllBookingsViewCatalogSingleton.Instance.AllBookings
                         join l in RoomsViewCatalogSingleton.Instance.RoomsView on r.Room_Id equals l.Room_Id
                         where r.Type == "Klasselokale" && l.City == LocationsVM.SelectedLocation.City
                         select r).ToList();

            foreach (var item in query)
            {
                TCPREF.BookingList.Add(item);
            }
        }
        /// <summary>
        /// The start list shows all the bookings
        /// </summary>
        public void ShowAllBookingList()
        {
            TCPREF.BookingList.Clear();
            var query = (from r in AllBookingsViewCatalogSingleton.Instance.AllBookings
                         join l in RoomsViewCatalogSingleton.Instance.RoomsView on r.Room_Id equals l.Room_Id
                         where r.Type == "Klasselokale" && l.City == LocationsVM.SelectedLocation.City
                         select r).ToList();

            foreach (var item in query)
            {
                TCPREF.BookingList.Add(item);
            }
        }
        #endregion

        #region Main Method
        /// <summary>
        /// This Method calls TeacherSnatchRoom if a series of conditions are met
        /// </summary>
        public async Task TeacherStealsBookingMethod()
        {
            var result = await DialogHandler.GenericYesNoDialog(
                $"Er du sikker på du vil booke dette lokale? {TCPREF.BookingIsSelected.RoomName}, ved at booke dette lokale aflyser du en elves booking",
                "Er du sikker?", "Ja", "Nej");
            if (result)
            {
                if (LoginHandler.SelectedUser.Teacher)
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
        #endregion

        #region Teacher-Check Method
        /// <summary>
        /// Bool method that decides if a booking within the specified timeinterval contains a teacher booking
        /// </summary>
        /// <returns></returns>
        public bool IsNotATeach()
        {
            var query = from t in UserCatalogSingleton.Instance.Users
                        join b in BookingCatalogSingleton.Instance.Bookings on t.User_Id equals b.User_Id
                        where b.Room_Id == TCPREF.BookingIsSelected.Room_Id && TCPREF.InputDate.Date == TCPREF.BookingIsSelected.Date.Date && b.Time_end >= TCPREF.InputTimeStart && b.Time_start <= TCPREF.InputTimeEnd
                        select t;
            if (query.Any(l => l.Teacher))
            {
                DialogHandler.Dialog("Den valgte booking indeholder en lærer-booking, det er desværre ikke muligt at slette en anden lærers booking", "lærer-booking fejl");
                return false;
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
                         where b.Room_Id == TCPREF.BookingIsSelected.Room_Id && TCPREF.InputDate.Date == TCPREF.BookingIsSelected.Date.Date && b.Time_end >= TCPREF.InputTimeStart && b.Time_start <= TCPREF.InputTimeEnd
                         select b).ToList();

            foreach (var item in query)
            {
                PersistancyService.DeleteFromDatabaseAsync("Bookings", item.Booking_Id);
                await GetMailToUser(item.User_Id, "En lærer aflyste din booking", $"Din booking den " +
                    $"{TCPREF.BookingIsSelected.Date.ToString("dd/MM/yyyy")} fra " +
                    $"{new DateTime(TCPREF.BookingIsSelected.BookingStart.Ticks).ToString("HH:mm")} til " +
                    $"{new DateTime(TCPREF.BookingIsSelected.BookingEnd.Ticks).ToString("HH:mm")} i rum {TCPREF.BookingIsSelected.RoomName} " +
                    $"er blevet aflyst af {LoginHandler.SelectedUser.User_Name}, vi beklager ulejligheden. " +
                    $"Du er selvfølgelig velkommen til at booke et nyt rum i appen.", true);
            }

            await PersistancyService.SaveInsertAsJsonAsync(new Booking
            {
                Date = TCPREF.InputDate.Date,
                Room_Id = TCPREF.BookingIsSelected.Room_Id,
                TavleBookings = null,
                Time_start = TCPREF.InputTimeStart,
                Time_end = TCPREF.InputTimeEnd,
                User_Id = LoginHandler.CurrentUserId
            }, "Bookings");
            ResetList();
        }
        #endregion 
        #endregion
    }
}

