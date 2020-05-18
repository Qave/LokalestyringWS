using LokalestyringUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LokalestyringUWP.Models.Singletons;
using Newtonsoft.Json;

namespace LokalestyringUWP.Service
{
    public class PersistancyService
    {
        public const string serverUrl = "http://localhost:51531";

        #region generic load table
        /// <summary>
        /// Generic method for loading data from the database via the specified tabel, via HTTP requests.
        /// </summary>
        /// <typeparam name="T">The type of the table you want to load data from. For exampel: Booking.</typeparam>
        /// <param name="uriIdentifier">The string that represents the table (in plural) that gets called in the URL to call HTTP requests. For exampel: api/Bookings, where "Bookings" needs to be specified in the uriIdentifier.</param>
        /// <returns></returns>
        public static async Task<ObservableCollection<T>> LoadTableFromJsonAsync<T>(string uriIdentifier)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                ObservableCollection<T> tableContents = new ObservableCollection<T>();
                try
                {
                    var response = client.GetAsync("api/" + uriIdentifier).Result;
                    var tableData = await response.Content.ReadAsAsync<IEnumerable<T>>();
                    if (response.IsSuccessStatusCode)
                    {
                        foreach (var row in tableData)
                        {
                            tableContents.Add(row);
                        }
                        return tableContents;
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

        #region Generic Insert
        /// <summary>
        /// A generic method for saving data to the database, via HTTP requests.
        /// </summary>
        /// <typeparam name="T">Type of the object that needs to be saved to the database.</typeparam>
        /// <param name="obj">The object that will get passed through and saved to the database.</param>
        /// <param name="uriIdentifier">The string that represents the table (in plural) that gets called in the URL to call HTTP requests. For exampel: api/Bookings, where "Bookings" needs to be specified in the uriIdentifier.</param>
        public static async Task<T> SaveInsertAsJsonAsync<T>(T obj, string uriIdentifier)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var response = await client.PostAsJsonAsync("api/"+ uriIdentifier, obj);
                    var result = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(result);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        #endregion

        #region Generic Delete
        /// <summary>
        /// Generic method that deletes a row from a given table (type) from the database, via HTTP requests.
        /// </summary>
        /// <param name="id">The ID that corresponds to the Booking that needs to be deleted (Comes from the SelectedBooking property in UserBookingsVM).</param>
        /// <param name="uriIdentifier">The string that represents the table (in plural) that gets called in the URL to call HTTP requests. For exampel: api/Bookings, where "Bookings" needs to be specified in the uriIdentifier.</param>
        public static void DeleteFromDatabaseAsync(string uriIdentifier, int id)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var response = client.DeleteAsync("api/"+uriIdentifier+"/"+id).Result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        #endregion

        #region Generic Update
        /// <summary>
        /// Generic method that updates a specific row in the database, via HTTP requests.
        /// </summary>
        /// <typeparam name="T">The type of the object that gets deleted from the database.</typeparam>
        /// <param name="obj">The object of type T that gets deleted from the database.</param>
        /// <param name="uriIdentifier">The string that represents the table (in plural) that gets called in the URL to call HTTP requests. For exampel: api/Bookings, where "Bookings" needs to be specified in the uriIdentifier.</param>
        /// <param name="id">The ID of the table row / selected object, that needs to be updated.</param>
        public static void UpdateAsJsonAsync<T>(T obj, string uriIdentifier, int id)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var response = client.PutAsJsonAsync("api/" + uriIdentifier + "/" + id, obj).Result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        #endregion

        #region Room Persistancy
        public static async void SaveRoomAsJsonAsync(Room roomObj)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(serverUrl);
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
        #endregion

        #region User Persistancy
        public static async void SaveUserAsJsonAsync(User userObj)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = client.PostAsJsonAsync("api/Users", userObj).Result;
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
                client.BaseAddress = new Uri(serverUrl);
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
                client.BaseAddress = new Uri(serverUrl);
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

        #region Building Persistancy
        public static async Task<ObservableCollection<Building>> LoadBuildingsFromJsonAsync()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                ObservableCollection<Building> buildingList = new ObservableCollection<Building>();

                try
                {
                    var response = await client.GetAsync("api/Buildings");
                    var buildings = response.Content.ReadAsAsync<IEnumerable<Building>>().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        foreach (var singleBuilding in buildings)
                        {
                            buildingList.Add(singleBuilding);
                        }
                        return buildingList;
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

        #region Booking Persistancy


        public static async void SaveBookingAsJsonAsync(Booking bookingObj)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = client.PostAsJsonAsync("api/Booking", bookingObj).Result;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        #endregion
    }
}
