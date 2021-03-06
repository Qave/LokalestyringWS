﻿using LokalestyringUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LokalestyringUWP.Models.Singletons;
using Newtonsoft.Json;
using LokalestyringUWP.Handler;
using Windows.UI.Xaml.Documents;

namespace LokalestyringUWP.Service
{
    public class PersistancyService
    {
        //// Use localhost Databaase (Portnumber will be unique to the host PC) PS: Will require a database called "LokalestyringDB", SQL script queries for this database can be found in the dokumentation for the project
        //public const string serverUrl = "http://localhost:51531/";

        //// Use the database located in the Azure Cloud, webservice hosted by qave.dk
        public const string serverUrl = "http://lokalestyringwebservice.qave.dk/"; 

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
                catch (ArgumentNullException e)
                {
                    DialogHandler.Dialog(e.Message+"\nPrøv at genstart programmet og prøv derefter igen","Parameter blev ikke fundet");
                }
                catch (HttpRequestException e)
                {
                    DialogHandler.Dialog(e.Message + "\nPrøv igen senere\nTjek eventuelt din forbindelse til internettet", "Ingen kontakt med serveren");
                }
                catch (Exception)
                {
                    DialogHandler.Dialog("Der gik noget galt ved sletning af dette object\nKontakt programmets administratorer for yderligere hjælp", "Noget gik galt");
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
                    var response = client.PostAsJsonAsync("api/"+ uriIdentifier, obj).Result;
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);
                }
                catch (Exception)
                {
                    DialogHandler.Dialog("Der gik noget galt ved sletning af dette object\nKontakt programmets administratorer for yderligere hjælp", "Noget gik galt");
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
                catch (ArgumentNullException e)
                {
                    DialogHandler.Dialog(e.Message+ "\nPrøv at genstart programmet og prøv derefter igen", "Parameter blev ikke fundet");                  
                }
                catch (InvalidOperationException e)
                {
                    DialogHandler.Dialog(e.Message+"\nKontant appens administratorer for yderligere hjælp","Ugyldig aktion");
                }
                catch (HttpRequestException e)
                {
                    DialogHandler.Dialog(e.Message+"\nPrøv igen senere\nTjek eventuelt din forbindelse til internettet","Ingen kontakt med serveren");
                }
                catch (Exception)
                {
                    DialogHandler.Dialog("Der gik noget galt ved sletning af dette object\nKontakt programmets administratorer for yderligere hjælp", "Noget gik galt");
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
        public static async Task UpdateAsJsonAsync<T>(T obj, string uriIdentifier, int id)
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
                    var response = await client.PutAsJsonAsync("api/" + uriIdentifier + "/" + id, obj);
                }
                catch (Exception)
                {
                    DialogHandler.Dialog("Der gik noget galt ved sletning af dette object\nKontakt programmets administratorer for yderligere hjælp", "Noget gik galt");
                }
            }
        }
        #endregion
    }
}
