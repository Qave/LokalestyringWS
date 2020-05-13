using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LokalestyringUWP.Common;
using LokalestyringUWP.Handler;
using LokalestyringUWP.Models;

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
            //UserCatalogSingleton.Instance.LoadUsersAsync();
            //UserCatalogSingleton abc = UserCatalogSingleton.Instance;
        }
        public ICommand LogOutCommand => CommandHandler.LoginCommand;
        public ICommand LoginCommand => CommandHandler.LoginCommand;
    }
}
