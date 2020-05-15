using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LokalestyringUWP.Common;
using LokalestyringUWP.Handler;

namespace LokalestyringUWP.ViewModel
{
    class BurgerMenuVM
    {
        public ICommand LogOutCommand => CommandHandler.LogOutCommand;

        public BurgerMenuVM()
        {
            CommandHandler.LogOutCommand = new RelayCommand(LoginHandler.OnLogout, null);
        }
    }
}
