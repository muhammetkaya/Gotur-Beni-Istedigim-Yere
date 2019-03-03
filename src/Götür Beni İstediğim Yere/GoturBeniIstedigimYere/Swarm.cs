using FourSquare.SharpSquare.Core;
using FourSquare.SharpSquare.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GoturBeniIstedigimYere
{
    public class Swarm
    {
        private string ClientID { get; set; }
        private string ClientSecret { get; set; }
        SharpSquare client;
        public Swarm(string clientID, string clientSecret)
        {
            this.ClientID = clientID;
            this.ClientSecret = clientSecret;
            client = new SharpSquare(ClientID, ClientSecret);
        }
        #region PUBLIC
        public List<Category> getCategories()
        {
            var categories = client.GetVenueCategories();
            return categories;
        }
        public async Task<List<PlaceResult.Venue>> venueSearch(string query, string location)
        {
            var json = await getVenueDetails(query, location, "10");
            var venues = JsonConvert.DeserializeObject<PlaceResult.RootObject>(json);
            return venues.response.venues;
        }
        public async Task<CheckInResult.Checkin> checkInSearch(string CheckinID)
        {
            var json = await getCheckinDetails(CheckinID);
            var venue = JsonConvert.DeserializeObject<CheckInResult.RootObject>(json);
            return venue.response.checkin;
        }
        public Venue VenueDetails(string venueID)
        {
            return client.GetVenue(venueID);

        }
        public User getUserInformation(string userID)
        {
            var user = client.GetUser(userID);
            return user;
        }
        #endregion
        #region HELPER
        private async Task<string> getCheckinDetails(string CheckinID)
        {
            string FoursquareApiURL = "https://api.foursquare.com/v2/checkins/resolve?";

            string query = FoursquareApiURL;
            query += "shortId=" + CheckinID;
            query += "&client_id=" + ClientID;
            query += "&client_secret=" + ClientSecret;
            query += "&v=" + String.Format("{0:yyyyMMdd}", DateTime.Now);
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(query).ConfigureAwait(false);
            }
        }
        private async Task<string> getVenueDetails(string VenueName, string near, string limit)
        {

            string FoursquareApiURL = "https://api.foursquare.com/v2//venues/search?";

            string query = FoursquareApiURL;
            query += "query=" + VenueName;
            query += "&near=" + near;
            query += "&limit=" + limit;
            query += "&client_id=" + ClientID;
            query += "&client_secret=" + ClientSecret;
            query += "&v=" + String.Format("{0:yyyyMMdd}", DateTime.Now);
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(query).ConfigureAwait(false);
            }
        }

        #endregion

        private void Test()
        {
            Swarm sw = new Swarm("", "");

        }
    }
}
