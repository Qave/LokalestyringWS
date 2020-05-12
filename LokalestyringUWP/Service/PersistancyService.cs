using LokalestyringUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LokalestyringUWP.Service
{
    class PersistancyService
    {
        #region Room Persistancy
        public static async void SaveRoomAsJsonAsync(Room roomObj)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(RoomCatalogSingleton.serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = client.PostAsJsonAsync("api/Rooms", roomObj).Result;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public static async Task<ObservableCollection<Room>> LoadRoomsFromJsonAsync()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(RoomCatalogSingleton.serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                ObservableCollection<Room> roomList = new ObservableCollection<Room>();

                try
                {
                    var response = await client.GetAsync("api/Rooms");
                    var rooms = response.Content.ReadAsAsync<IEnumerable<Room>>().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        foreach (var singleRoom in rooms)
                        {
                            roomList.Add(singleRoom);
                        }
                        return roomList;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return null;
        }
        #endregion

        #region User Persistancy
        public static async void SaveUserAsJsonAsync(User userObj)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(UserCatalogSingleton.serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = client.PostAsJsonAsync("api/User", userObj).Result;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public static async Task<ObservableCollection<User>> LoadUsersFromJsonAsync()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(UserCatalogSingleton.serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                ObservableCollection<User> userList = new ObservableCollection<User>();

                try
                {
                    var response = await client.GetAsync("api/Users");
                    var users = response.Content.ReadAsAsync<IEnumerable<User>>().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        foreach (var singleUser in users)
                        {
                            userList.Add(singleUser);
                        }
                        return userList;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return null;
        }
        #endregion

        #region Location Persistancy

        public static async Task<ObservableCollection<Location>> LoadLocationsFromJsonAsync()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("http://localhost:51531");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                ObservableCollection<Location> locationList = new ObservableCollection<Location>();

                try
                {
                    var response = await client.GetAsync("api/Locations");
                    var locations = response.Content.ReadAsAsync<IEnumerable<Location>>().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        foreach (var singleLocation in locations)
                        {
                            locationList.Add(singleLocation);
                        }
                        return locationList;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return null;
        }

        #endregion
    }
}
