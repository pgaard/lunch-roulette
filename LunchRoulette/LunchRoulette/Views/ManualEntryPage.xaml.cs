using System;
using LunchRoulette.Models;
using LunchRoulette.Services;
using Xamarin.Forms;

namespace LunchRoulette.Views
{  
    public partial class ManualEntryPage : ContentPage
    {
        public ManualEntryPage()
        {
            InitializeComponent();
        }

        private async void SaveButton_OnClicked(object sender, EventArgs e)
        {
            var lunch = new Lunch
            {
                RestaurantName = this.RestaurantName.Text,
                Address = this.Address.Text,
                Date = this.Date.Date,
                GroupId = Config.GroupId.ToString()
            };

            var lunchService = new LunchService();
            var count = await lunchService.Add(lunch);

            if (count == 0)
            {
                await DisplayAlert("Add", "Add failed!", "OK");
            }
            else
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }
    }
}
