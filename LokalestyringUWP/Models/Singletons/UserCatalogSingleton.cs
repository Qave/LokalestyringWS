﻿using System;
using System.Collections.ObjectModel;
using LokalestyringUWP.Service;

namespace LokalestyringUWP.Models.Singletons
{
    public class UserCatalogSingleton
    {
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
            ObservableCollection<User> users = await PersistancyService.LoadTableFromJsonAsync<User>("Users");
            try
            {
                if (users == null)
                {
                    throw new NullReferenceException();
                }
                else
                {
                    foreach (var item in users)
                    {
                        this.Users.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                //Another catch catches this exception
            }

        }
    }
}
