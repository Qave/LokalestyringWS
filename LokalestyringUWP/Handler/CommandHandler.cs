using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LokalestyringUWP.Common;

namespace LokalestyringUWP.Handler
{
    public static class CommandHandler
    {
        public static ICommand LoginCommand { get; set; }
        public static ICommand LogOutCommand { get; set; }
    }
}
