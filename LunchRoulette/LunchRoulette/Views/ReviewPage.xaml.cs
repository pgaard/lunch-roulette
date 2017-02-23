using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunchRoulette.Models;
using Xamarin.Forms;

namespace LunchRoulette.Views
{
    public partial class ReviewPage : ContentPage
    {
        public RestaurantDetail Restaurant { get; set; }

        public ReviewPage(RestaurantDetail restaurant)
        {
            this.Restaurant = restaurant;
            this.Title = restaurant.name;
            InitializeComponent();

            this.BindingContext = this;
        }
    }
}
