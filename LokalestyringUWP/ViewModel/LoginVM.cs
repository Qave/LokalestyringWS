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
            CommandHandler.LogOutCommand = new RelayCommand(LoginHandler.OnLogout,null);
            CommandHandler.LoginCommand = new RelayCommand(LoginHandler.OnLogin, null);
            CommandHandler.CreateAccountCommand = new RelayCommand(LoginHandler.OnCreateAccount, null);
            //UserCatalogSingleton.Instance.LoadUsersAsync();
            UserCatalogSingleton abc = UserCatalogSingleton.Instance;
        }
       //These properties are for binding in the UWP project
        public ICommand LogOutCommand => CommandHandler.LogOutCommand;
        public ICommand LoginCommand => CommandHandler.LoginCommand;
        public ICommand CreateAccountCommand => CommandHandler.CreateAccountCommand;
    }
}
