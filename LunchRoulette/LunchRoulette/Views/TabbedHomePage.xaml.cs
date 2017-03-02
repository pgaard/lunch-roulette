using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunchRoulette.Services;
using Xamarin.Forms;

namespace LunchRoulette.Views
{
    public partial class TabbedHomePage : TabbedPage
    {
        private readonly LunchService lunchService;

        public TabbedHomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);            
            this.Children.Add(new DiscoverPage());
            this.Children.Add(new LunchListPage());
            this.lunchService = new LunchService();
        }

        private async void Clear_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Delete all data", "Are you sure you want to delete all data?", "Yes", "No"))
            {
                this.lunchService.DeleteAll();
            }        
        }

        private async void Manual_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ManualEntryPage());
        }

        private void Everything_Clicked(object sender, EventArgs e)
        {
            
            PlacesService.FindRestaurantsOnly = !PlacesService.FindRestaurantsOnly;
            (sender as ToolbarItem).Text = PlacesService.FindRestaurantsOnly ? "Search Restaurants" : "Search Everything";
        }
    }
}
