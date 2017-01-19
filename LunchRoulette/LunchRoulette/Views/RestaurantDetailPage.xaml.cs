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
        private readonly LunchService lunchService;
        public Restaurant Restaurant { get; set; }

        public RestaurantDetailPage(Restaurant restaurant)
        {
            this.lunchService = new LunchService();
            this.Restaurant = restaurant;
            this.BindingContext = this;
            this.InitializeComponent();
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
