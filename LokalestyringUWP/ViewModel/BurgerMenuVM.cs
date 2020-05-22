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
        public ICommand LogOutCommand { get; set; }
        public ICommand GoToUserBookingsCommand { get; set; }
        public ICommand GoToTecherControlPanel { get; set; }

        public BurgerMenuVM()
        {
            LogOutCommand = new RelayCommand(LoginHandler.Logout, null);
            GoToUserBookingsCommand = new RelayCommand(GoToUserBookingsMethod, null);
            GoToTecherControlPanel = new RelayCommand(GoToTecherControlPanelMethod, null);
        }
        public void GoToUserBookingsMethod()
        {
            ((Frame)Window.Current.Content).Navigate(typeof(PageUserBookings));
        }
        public void GoToTecherControlPanelMethod()
        {
            ((Frame)Window.Current.Content).Navigate(typeof(PageTeacherControlPanel));
        }
        public Visibility HideGoToTecherControlPanelBtn =>
            UserHandler.CurrentUserTeacher && LocationsVM.SelectedLocation != null
                ? Visibility.Visible
                : Visibility.Collapsed;
    }
}
