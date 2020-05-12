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
    }
}
