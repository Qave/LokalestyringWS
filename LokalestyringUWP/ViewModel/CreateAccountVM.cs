using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LokalestyringUWP.Common;
using LokalestyringUWP.Handler;
using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;
using LokalestyringUWP.Service;
using LokalestyringUWP.View;

namespace LokalestyringUWP.ViewModel
{
    class CreateAccountVM
    {
        /// <summary>
        /// These properties are for binding in the UWP project
        /// </summary>
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsTeacher { get; set; }

        public ICommand GoBackCommand => CommandHandler.GoBackCommand;
        public ICommand CreateAccountCommand { get; set; }


        public CreateAccountVM()
        {
            CommandHandler.GoBackCommand = new RelayCommand(GoBackMethod, null);
            CreateAccountCommand = new RelayCommand(CreateAccount, null);
        }

        /// <summary>
        /// Creates a new account and it checks if everything has been filled in if it has it creates a new user
        /// and sends it to PersistancyService.SaveInsertAsJsonAsync then refreshes the UserCatalogSingleton
        /// and then navigates to PageLogin so the user can login.
        /// </summary>
        private void CreateAccount()
        {
            if (UserName != null && Email != null && Password != null)
            {
                if (EmailAvailability(Email))
                {
                    DialogHandler.Dialog("Brugernavnet findes allerede", "Fejlet oprettelse");
                }
                else
                {
                    PersistancyService.SaveInsertAsJsonAsync(new User() {User_Name = UserName, User_Email = Email, Password = Password, Teacher = IsTeacher}, "Users");
                    UserCatalogSingleton.Instance.LoadUsersAsync();
                    ((Frame)Window.Current.Content).Navigate(typeof(PageLogin));
                }
            }
            else
            {
                DialogHandler.Dialog("Mangler at udfylde brugernavn, email eller password", "Fejlet oprettelse");
            }
        }

        /// <summary>
        /// Checks if the username already exists and returns true or false.
        /// </summary>
        /// <param name="mail"></param>
        /// <returns>true or false</returns>
        private bool EmailAvailability(string mail)
        {
            if (UserCatalogSingleton.Instance.Users.Any(p => p.User_Email == mail))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns to last page (PageLogin)
        /// </summary>
        private void GoBackMethod()
        {
            ((Frame)Window.Current.Content).GoBack();
        }
    }
}
