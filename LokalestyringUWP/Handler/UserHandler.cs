using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;
using LokalestyringUWP.View;

namespace LokalestyringUWP.Handler
{
    public class UserHandler
    {
        //Properties for the CurrentUserTeacher and CurrentUserName
        public static bool CurrentUserTeacher { get; set; }
        public static string CurrentUserName { get; set; }
        //Default Constructor
        public UserHandler()
        {
            
        }
        //Checks the UserSingleTon for username availability
        public static bool UsernameAvailble(string username)
        {
            if (UserCatalogSingleton.Instance.Users.Any(u => u.User_Name == username))
            {
                return true;
            }
            else return false;
        }
        //The Errordialog can be called when the program has run into an error to tell the user what's wrong
        public static async void ErrorDialog(string message, string title)
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
                ((Frame) Window.Current.Content).Navigate(typeof(PageLogin));
            }
            else
            {
                //do nothing
            }
        }
    }
}
