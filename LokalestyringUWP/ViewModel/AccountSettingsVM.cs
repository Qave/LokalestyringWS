using LokalestyringUWP.Common;
using LokalestyringUWP.Handler;
using LokalestyringUWP.Models;
using LokalestyringUWP.Models.Singletons;
using LokalestyringUWP.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace LokalestyringUWP.ViewModel
{
    public class AccountSettingsVM : INotifyPropertyChanged
    {
        private string _userName;
        private string _user_Email;
        private string _password;
        private bool _teacher;

        private bool _nameCanBeEditted;
        private bool _emailCanBeEditted;
        public AccountSettingsVM()
        {
            _userName = LoginHandler.SelectedUser.User_Name;
            _user_Email = LoginHandler.SelectedUser.User_Email;
            _password = LoginHandler.SelectedUser.Password;
            _teacher = LoginHandler.SelectedUser.Teacher;

            EditNameCommand = new RelayCommand(EditNameMethod, null);
            EditEmailCommand = new RelayCommand(EditEmailMethod, null);
            SaveChangesCommand = new RelayCommand(async () => await SaveChangesMethodAsync(), null);

            _nameCanBeEditted = false;
            _emailCanBeEditted = false;
        }

        //Binding Properties for editing the user data
        public string UserNameBinding
        {
            get { return _userName; }
            set { _userName = value; }
        }
        public string UserEmailBinding
        {
            get { return _user_Email; }
            set { _user_Email = value; }
        }
        public string PasswordBinding
        {
            get { return _password; }
            set { _password = value; }
        }
        public string TeacherBinding
        {
            get
            {
                if (_teacher)
                {
                    return "Ja";
                }
                else
                {
                    return "Nej";
                }
            }
        }


        // ICommands for button bindings
        public ICommand EditNameCommand { get; set; }
        public ICommand EditEmailCommand { get; set; }
        public ICommand EditPasswordCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }

        // Enabled properties
        public bool NameCanBeEditted { get { return _nameCanBeEditted; } set { _nameCanBeEditted = value; OnPropertyChanged(); } }
        public bool EmailCanBeEditted { get { return _emailCanBeEditted; } set { _emailCanBeEditted = value; OnPropertyChanged(); } }

        // Visibility for Password change

        public Visibility PasswordEditVisibility { get; set; }

        // Methods for enabling editting

        public void EditNameMethod() 
        {
            EmailCanBeEditted = false;
            NameCanBeEditted = true;

        }
        public void EditEmailMethod()
        {
            NameCanBeEditted = false;
            EmailCanBeEditted = true;
        }

        /// <summary>
        /// Async method that saves the current changes to the user data to the database, and refreshes the User Singleton
        /// </summary>
        /// <returns>Task</returns>
        public async Task SaveChangesMethodAsync()
        {
            User userObj = new User() {User_Id = LoginHandler.CurrentUserId, User_Name = UserNameBinding, User_Email = UserEmailBinding, Password = PasswordBinding, Teacher = _teacher };
            await PersistancyService.UpdateAsJsonAsync(userObj, "Users", LoginHandler.CurrentUserId);
            refreshUserList();
            OnPropertyChanged(nameof(UserNameBinding));
            OnPropertyChanged(nameof(UserEmailBinding));
            NameCanBeEditted = false;
            EmailCanBeEditted = false;
        }
        /// <summary>
        /// Refreshes the user list
        /// </summary>
        public void refreshUserList()
        {
            UserCatalogSingleton.Instance.Users.Clear();
            UserCatalogSingleton.Instance.LoadUsersAsync();
        }
        #region INotifyPropertyChanged interface implementation
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Refreshes the property on the pageview.
        /// </summary>
        /// <param name="propertyName">You can specify the property to update when using "nameof(propertyName)" as a parameter</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
