using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LokalestyringUWP.Annotations;
using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;
using LokalestyringUWP.Service;
using LokalestyringUWP.View;
using LokalestyringUWP.ViewModel;

namespace LokalestyringUWP.Handler
{
    public static class LoginHandler
    {
        
        //This is a global property, so we always know who is logged in
        public static int CurrentUserId { get; set; }
        //This is a bool that shows if the user is a teacher or not, Teachers have more power
        public static bool IsTeacher { get; set; }
        //Properties for logging in
        public static string UserName { get; set; }
      
        public static string Password { get; set; }
       

        //This method checks for correct password and username, This is however not very secure
        // It does this by checking if password and username inputted matches a users password and username in the database.
        private static bool CheckUserCredentials
        {
            get
            {
                foreach (var user in UserCatalogSingleton.Instance.Users)
                {
                    if (user.User_Name == UserName && user.Password == Password)
                    {
                            UserHandler.CurrentUserName = user.User_Name;
                            UserHandler.CurrentUserTeacher = user.Teacher;
                            CurrentUserId = user.User_Id;
                            IsTeacher = user.Teacher;
                            return true;
                    }
                }
                return false;
            }
        }
        //this method has to be bound to the login button, in the PageLogin view
        public static void OnLogin()
        {
            UserCatalogSingleton.Instance.LoadUsersAsync();
            if (UserName != null && Password != null) // Makes sure that password and username is filled out
            {
                if (CheckUserCredentials) // Checks if password and username inputted matches in database.
                {
                    ((Frame) Window.Current.Content).Navigate(typeof(PageLocations)); //redirect to the next page
                }
                else
                {
                    Password = ""; // this call clears the password box's textfield by Clearing the two-way-binded property UserName.
                    UserHandler.ErrorDialog("Forkert brugernavn eller password", "Fejlet login"); // Error MessageBox
                }
            }
            else
            {
                UserHandler.ErrorDialog("Forkert brugernavn eller password", "Login fejlet"); //Error message
            }
        }

        public static void OnLogout()
        {
            UserHandler.LogOutDialog("Er du sikker på du vil logge ud?", "Log ud");
        }




    }
}

