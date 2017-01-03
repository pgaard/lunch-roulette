﻿using System;
using System.Collections.Generic;
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

    public partial class DiscoverPage : ContentPage
    {
        private PlacesService service;
        private LunchService lunchService;

        public ObservableCollection<Restaurant> Restaurants;


        public DiscoverPage()
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
            this.Spinner.IsRunning = true;
            this.Spinner.IsVisible = true;
            this.Winner.IsVisible = false;

            var restaurants =
                await
                    this.service.GetRestaurants(this.MyMap.VisibleRegion.Center.Latitude,
                        this.MyMap.VisibleRegion.Center.Longitude);
            if (restaurants != null && restaurants.results.Count > 0)
            {
                var filtered = await FilterRestaurants(restaurants.results);

                this.Counts.Text = $"{filtered.Count} restaurants left out of {restaurants.results.Count}";                

                var winners = GetRandomRestaurants(filtered, 3);

                MyMap.Pins.Clear();
                foreach (var winner in winners)
                {
                    MyMap.Pins.Add(new Pin()
                    {
                        Position =
                            new Position(winner.geometry.location.lat, winner.geometry.location.lng),
                        Label = winner.name,
                        Type = PinType.Place
                    });
                }

                this.Restaurants = new ObservableCollection<Restaurant>(winners);
                chowList.ItemsSource = this.Restaurants;
            }

            this.Spinner.IsRunning = false;
            this.Spinner.IsVisible = false;
            this.Winner.IsVisible = true;
        }

        private async Task<List<Restaurant>> FilterRestaurants(List<Restaurant> restaurants)
        {
            var stopwords = new[] { "caribou" };
            var previous = await lunchService.GetAll();

            var filtered = restaurants.Where(r => !stopwords.Any(s => r.name.ToLower().Contains(s)) && previous.All(p => p.GoogleId != r.id));
           
            return filtered.ToList();
        }

        private List<Restaurant> GetRandomRestaurants(List<Restaurant> restaurants, int number)
        {
            if (restaurants.Count < number)
            {
                number = restaurants.Count;
            }

            var rnd = new Random();
            var result = new List<Restaurant>();
            while (result.Count < number)
            {
                var random = restaurants[rnd.Next(0, restaurants.Count)];
                if (result.All(r => r.id != random.id))
                {
                    result.Add(random);
                }
            }

            return result;
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
                if (await DisplayAlert("Choose", $"Are we really eating at {restaurant.name}?", "Yes", "No"))
                {
                    var lunch = new Lunch()
                    {
                        RestaurantName = restaurant.name,
                        Address = restaurant.vicinity,
                        GoogleId = restaurant.id,
                        Date = DateTime.Now.Date
                    };
                    var count = await this.lunchService.Add(lunch);

                    if (count == 0)
                    {
                        await DisplayAlert("Add", "Add failed!", "OK");
                    }
                }

                // todo deselect
            }
        }
    }
}
