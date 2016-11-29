using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using ContactsFP.Data;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ContactsFP.Services
{
    public static class WebService
    {

        #region Non-public members
        /// <summary>
        /// The web addresses of the endpoints to target. The first one is the default and
        /// the second one is used if the first is unreachable.
        /// </summary>
        private static readonly string[] WebserviceUrls =
        {
            Constants.registerUrl,
            Constants.countriesUrl
        };

        /// <summary>
        /// Tracks wich endpoint is currently being used.
        /// </summary>
        private static int RegisterEndpoint = 0;
        private static int ContriesEndpoint = 1;

        /// <summary>
        /// Timeout in milliseconds for short requests.
        /// </summary>
        private const int ShortTimeout = 300000;

        /// <summary>
        /// Timeout in milliseonds for lengthy requests such as image uploads.
        /// </summary>
        private const int LongTimeout = 1500000;

        /// <summary>
        /// Contains the names of the server methods to be invoked for requests.
        /// </summary>

        #endregion

        /// <summary>
        /// Returns the address of the service that is currently being used.
        /// </summary>
        /// <returns></returns>
        public static string CurrentEndpointUrl()
        {
            return WebserviceUrls[RegisterEndpoint];
        }

        public static async Task<List<Country>> GetCountries()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMilliseconds(ShortTimeout);
                var uri = new Uri(WebserviceUrls[ContriesEndpoint]);
                client.BaseAddress = uri;
                HttpResponseMessage response = await client.GetAsync(uri);

                if (GoodResponse(response.StatusCode))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ListJson = JsonConvert.DeserializeObject<List<Country>>(content);
                    return ListJson;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }


        public static async Task<bool> CreateContacts(ContactsDataModel contacts)
        {
            try
            {
                ContactsModel contactsModel = new ContactsModel();
                contactsModel.Location = contacts.Location;
                contactsModel.Contacts = contacts.Contacts.ToList<Contact>();

                var json = JsonConvert.SerializeObject(contactsModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMilliseconds(ShortTimeout);
                var uri = new Uri(WebserviceUrls[RegisterEndpoint]);
                client.BaseAddress = uri;

                HttpResponseMessage response = await new HttpClient().PostAsync(uri, content);

                if (GoodResponse(response.StatusCode))
                {
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Confirmación", ex.Message, "OK");
                return false;
            }
        }

        private static bool GoodResponse(System.Net.HttpStatusCode status)
        {
            if (status == System.Net.HttpStatusCode.Created
                || status == System.Net.HttpStatusCode.OK
                || status == System.Net.HttpStatusCode.Accepted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
