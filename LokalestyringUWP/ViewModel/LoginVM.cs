using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LokalestyringUWP.Common;
using LokalestyringUWP.Handler;
using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;

namespace LokalestyringUWP.ViewModel
{
    public class LoginVM
    {
        public string PasswordVM
        {
            get { return LoginHandler.Password;}
            set { LoginHandler.Password = value; }
        }

        public string UserNameVM
        {
            get { return LoginHandler.UserName;}
            set { LoginHandler.UserName = value; }
        } 
        

       public LoginVM()
        {
            LogOutCommand = new RelayCommand(LoginHandler.Logout,null);
            LoginCommand = new RelayCommand(LoginHandler.Login, null);
            CreateAccountCommand = new RelayCommand(LoginHandler.CreateAccount, null);
            //UserCatalogSingleton.Instance.LoadUsersAsync();
            UserCatalogSingleton abc = UserCatalogSingleton.Instance;
        }
       //These properties are for binding in the UWP project
        public ICommand LogOutCommand { get; set; } 
        public ICommand LoginCommand { get; set; }
        public ICommand CreateAccountCommand { get; set; }
    }
}
