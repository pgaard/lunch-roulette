using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LunchRoulette.Models;
using LunchRoulette.Services;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;

namespace LunchRoulette.Views
{
    using Xamarin.Forms;

    public partial class LandingPage : ContentPage
    {
        private PlacesService service;
        private LunchService lunchService;

        public ObservableCollection<Restaurant> Restaurants;
       

        public LandingPage()
        {
            this.service = new PlacesService();
            this.lunchService = new LunchService();
            InitializeComponent();
            //Task.Run(this.UpdateMap);
        }

        public async Task UpdateMap()
        {
            var location = await GetCurrentPosition();
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMiles(1)));
        }

        public async Task<Position> GetCurrentPosition()
        {
            try
            {   
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var location = await locator.GetPositionAsync(10000);
                return new Position(location.Latitude, location.Longitude);
            }
            catch (Exception ex)
            {
                var geoCoder = new Geocoder();
                var location = await geoCoder.GetPositionsForAddressAsync("4916 40th Ave S Minneapolis MN 55417");
                return location.FirstOrDefault();
            }
        }

        public async void Button_Clicked(object sender, EventArgs eventArgs)
        {
            var restaurants = await this.service.GetRestaurants(this.MyMap.VisibleRegion.Center.Latitude, this.MyMap.VisibleRegion.Center.Longitude);
            if (restaurants != null && restaurants.results.Count > 0)
            {
                this.Restaurants = new ObservableCollection<Restaurant>(restaurants.results);

                var rnd = new Random();
                var randomRestaurant = restaurants.results[rnd.Next(0, restaurants.results.Count)];

                this.Winner.Text = "Winner: " + randomRestaurant.name;
                MyMap.Pins.Add(new Pin()
                {
                    Position = new Position(randomRestaurant.geometry.location.lat, randomRestaurant.geometry.location.lng),
                    Label = randomRestaurant.name
                });

                chowList.ItemsSource = this.Restaurants;
            }                        
        }
      
        private async void Button_Map(object sender, EventArgs e)
        {
            await this.UpdateMap();
        }

        private async void Restaurant_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            var restaurant = e.SelectedItem as Restaurant;

            if (restaurant != null)
            {
                var lunch = new Lunch()
                {
                    RestaurantName = restaurant.name,
                    Address = restaurant.vicinity,
                    GoogleId = restaurant.id,
                    Date = DateTime.Now.Date
                };

                var id = await this.lunchService.Add(lunch);
                 
                await DisplayAlert("test", "Add lunch " + id, "OK");
            }
        }
    }
}
