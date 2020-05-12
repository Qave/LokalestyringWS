using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using LokalestyringUWP.Models;
using LokalestyringUWP.Service;

namespace LokalestyringUWP.Handler
{
    public class LoginHandler
    {
        //This is a global property, so we always know who is logged in
        public static int CurrentUserId { get; set; }
        //This is a bool that shows if the user is a teacher or not, Teachers have more power
        public bool IsTeacher { get; set; }
        //Properties for logging in
        public string UserName { get; set; }
        public string Password { get; set; }
        //This method checks for correct password and username, This is however not very secure

        private bool CheckUserCredentials
        {
            get
            {
                foreach (var user in UserCatalogSingleton.Instance.Users)
                {
                    
                    


                }
            }
        }




        public void CheckLoginCredentials()
        {
            if (UserName != null && Password != null) // Makes sure that password and username is filled out
            {
                if ()
                {
                }





            }
            else
            {
                ErrorDialog("Forkert brugernavn eller password", "Login fejlet"); //Error message
            }
        }


       
        }
    }
}
}
