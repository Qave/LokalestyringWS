using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LokalestyringUWP.Models;
using LokalestyringUWP.View;

namespace LokalestyringUWP.Handler
{
    public class DialogHandler
    {
        /// <summary>
        /// A generic dialog popup, that display whenever theres a need for a Yes/No popup, returns true or false
        /// </summary>
        /// <param name="message">The message that needs to be send to the user when the dialog is called</param>
        /// <param name="title">The Title of the dialog</param>
        /// <param name="yes">The positive choice of the dialog box (YES)</param>
        /// <param name="no">The Negative choice of the dialog box (NO)</param>
        /// <returns>bool</returns>
        public static async Task<bool> GenericYesNoDialog(string message, string title, string yes, string no)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                PrimaryButtonText = yes,
                SecondaryButtonText = no,
                DefaultButton = ContentDialogButton.Close
            };
            ContentDialogResult result = await contentDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static async void Dialog(string message, string title)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "Ok",
                DefaultButton = ContentDialogButton.Close
            };
            ContentDialogResult result = await contentDialog.ShowAsync();
        }

        public static async void LogOutDialog(string message, string title)
        {
            ContentDialog YesNoDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "Ja",
                CloseButtonText = "Nej"

            };
            ContentDialogResult resultYesNo = await YesNoDialog.ShowAsync();
            if (resultYesNo == ContentDialogResult.Primary)
            {
                ((Frame)Window.Current.Content).Navigate(typeof(PageLogin));
            }
            else
            {
                //do nothing
            }
        }

        public static async void ConfirmBookingDialog()
        {
            ContentDialog confirmBookingDialog = new ContentDialog
            {
                Title = "Book dette lokale?",
                Content = "Er du sikker på du vil booke dette lokale?", //+Lokalets navn!?!?!,
                PrimaryButtonText = "Book",
                CloseButtonText = "Book ikke"

            };
            ContentDialogResult result = await confirmBookingDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                BookingHandler.CreateBooking();
            }
            else
            {
                //do nothing
            }
        }
    }
}
