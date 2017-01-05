using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LunchRoulette.Models;
using Newtonsoft.Json;
using PCLAppConfig;

namespace LunchRoulette.Services
{
    public class PlacesService
    {
        private readonly HttpClient client;

        private readonly string googlePlacesApiKey;

        private const string Url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?key={0}&location={1},{2}&rankby=distance&type=restaurant&opennow";

        public PlacesService()
        {
            this.client = new HttpClient();
            googlePlacesApiKey = Config.GooglePlacesApiKey;
        }

        public async Task<Restaurants> GetRestaurants(double latitude = 44.9135599, double longitude = -93.21555699999999)
        {
            try
            {
                var response = await this.client.GetAsync(new Uri(String.Format(Url, this.googlePlacesApiKey, latitude, longitude)));
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var restaurants = JsonConvert.DeserializeObject<Restaurants>(result);
                    return restaurants;
                }
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }
    }
}
