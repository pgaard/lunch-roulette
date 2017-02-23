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

        public static bool FindRestaurantsOnly = true;

        private const string Url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?key={0}&location={1},{2}&rankby=distance&type=restaurant";
        private const string UrlAny = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?key={0}&location={1},{2}&rankby=distance";
        private const string DetailUrl = "https://maps.googleapis.com/maps/api/place/details/json?placeid={1}&key={0}";

        public PlacesService()
        {
            this.client = new HttpClient();
            googlePlacesApiKey = Config.GooglePlacesApiKey;
        }

        public async Task<Restaurants> GetRestaurants(double latitude, double longitude)
        {
            try
            {
                var response = await this.client.GetAsync(new Uri(String.Format(FindRestaurantsOnly ? Url : UrlAny, this.googlePlacesApiKey, latitude, longitude)));
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

        public async Task<RestaurantDetail> GetRestaurantDetail(string id)
        {
            var response = await this.client.GetAsync(new Uri(String.Format(DetailUrl, this.googlePlacesApiKey, id)));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var restaurantRoot = JsonConvert.DeserializeObject<RestaurantDetailRootObject>(result);
                return restaurantRoot.result;
            }
            return null;
        }

        public string PhotoUrl(Photo photo, int maxWidth)
        {
            return String.Empty;
        }
    }
}
