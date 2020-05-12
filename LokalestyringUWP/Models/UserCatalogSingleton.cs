using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LokalestyringUWP.Service;

namespace LokalestyringUWP.Models
{
    public class UserCatalogSingleton
    {
            public const string serverUrl = "http://localhost:51531";
            private static UserCatalogSingleton _instance = null;

            public ObservableCollection<User> Users { get; }
            public static UserCatalogSingleton Instance { get { return _instance ?? (_instance = new UserCatalogSingleton()); } }

            public UserCatalogSingleton()
            {
                Users = new ObservableCollection<User>();
                LoadUsersAsync();
            }

            public async void LoadUsersAsync()
            {
                ObservableCollection<User> users = await PersistancyService.LoadUsersFromJsonAsync();

                foreach (var item in users)
                {
                    this.Users.Add(item);
                }
            }
        }
}
