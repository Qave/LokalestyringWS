using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using LokalestyringUWP.Models;

namespace LokalestyringUWP.Handler
{
    public class UserHandler
    {
        public static bool CurrentUserTeacher { get; set; }
        public static string CurrentUserName { get; set; }

        public UserHandler()
        {
            
        }


        public static bool UsernameAvailble(string username)
        {
            if (UserCatalogSingleton.Instance.Users.Any(u => u.User_Name == username))
            {
                return true;
            }
            else return false;
        }


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
    }
}
