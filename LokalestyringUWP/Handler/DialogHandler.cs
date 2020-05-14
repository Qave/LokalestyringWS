using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LokalestyringUWP.View;

namespace LokalestyringUWP.Handler
{
    public class DialogHandler
    {
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
                
            }
            else
            {
                //do nothing
            }
        }
    }
}
