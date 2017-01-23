using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunchRoulette.Models;
using Xamarin.Forms;

namespace LunchRoulette.Services
{
    public partial class RestaurantDetailPage : ContentPage
    {
        private string PlacesPhotoApiUrl = "https://maps.googleapis.com/maps/api/place/photo?maxwidth={0}&photoreference={1}&key={2}";
        private readonly LunchService lunchService;
        public RestaurantDetail Restaurant { get; set; }

        public RestaurantDetailPage(RestaurantDetail restaurant)
        {
            this.lunchService = new LunchService();
            this.Restaurant = restaurant;
            this.Title = restaurant.name;

            if (restaurant.photos != null && restaurant.photos.Count > 0)
            {
                foreach (var photo in restaurant.photos)
                {
                    photo.ImageSource = new UriImageSource
                    {
                        Uri = new Uri(string.Format(PlacesPhotoApiUrl, 500, photo.photo_reference, Config.GooglePlacesApiKey)),
                        CachingEnabled = true
                    };
                }
            }
                    
            this.BindingContext = this;
            this.InitializeComponent();

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                Device.OpenUri(new Uri(restaurant.url));
            };
            this.UrlLink.GestureRecognizers.Add(tapGestureRecognizer);
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            if (Restaurant != null)
            {
                if (await DisplayAlert("Choose", $"Are we really eating at {Restaurant.name}?", "Yes", "No"))
                {
                    var lunch = new Lunch()
                    {
                        RestaurantName = Restaurant.name,
                        GroupId = Config.GroupId.ToString(),
                        Address = Restaurant.vicinity,
                        GoogleId = Restaurant.id,
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
