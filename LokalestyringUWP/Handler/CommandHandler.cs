using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LokalestyringUWP.Common;

namespace LokalestyringUWP.Handler
{
    public class CommandHandler
    {
        LoginHandler login = new LoginHandler();
        public ICommand LoginCommand { get; set; }

        public CommandHandler()
        {
            LoginCommand = new RelayCommand(login.OnLogin, null);
        }



    }
}
