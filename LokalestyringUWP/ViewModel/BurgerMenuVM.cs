using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LokalestyringUWP.Common;
using LokalestyringUWP.Handler;
using LokalestyringUWP.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LokalestyringUWP.ViewModel
{
    class BurgerMenuVM
    {
        public ICommand LogOutCommand => CommandHandler.LogOutCommand;
        public ICommand GoToUserBookingsCommand { get; set; }

        public BurgerMenuVM()
        {
            CommandHandler.LogOutCommand = new RelayCommand(LoginHandler.OnLogout, null);
            //GoToUserBookingsCommand = new RelayCommand(GoToUserBookingsMethod, null);
        }
        //public void GoToUserBookingsMethod()
        //{
        //    ((Frame)Window.Current.Content).Navigate(typeof(PageUserBookings));
        //}
        public Visibility HideGoToUserBookingsBtn { get; set; }
    }
}
