using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LunchRoulette.Models;
using LunchRoulette.Services;
using LunchRoulette.Views;
using Xamarin.Forms;

namespace LunchRoulette
{
    public partial class LunchListPage : ContentPage
    {
        public ObservableCollection<Lunch> Lunches { get; set; }
        private readonly LunchService lunchService;

        public LunchListPage()
        {
            this.lunchService = new LunchService();
            InitializeComponent();
        }

        private async Task Load()
        {
            try
            {
                var lunches = await this.lunchService.GetAll();
                this.Lunches = new ObservableCollection<Lunch>(lunches);
                this.LunchList.ItemsSource = this.Lunches.OrderByDescending(l => l.Date);
            }
            catch (Exception ex)
            {

            }
        }

        protected override async void OnAppearing()
        {
            await Load();
            base.OnAppearing();
        }

        private async void Delete_Lunch(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var lunch = menuItem.CommandParameter as Lunch;
            if (lunch != null)
            {
                await this.lunchService.Delete(lunch);
                var deleted = this.Lunches.FirstOrDefault(l => l.Id == lunch.Id);
                this.Lunches.Remove(deleted);
            }
        }

        private async void LunchList_OnItemTapped(object sender, ItemTappedEventArgs itemTappedEventArgs)
        {
            (sender as ListView).SelectedItem = null;

            var lunch = itemTappedEventArgs.Item as Lunch;

            await Application.Current.MainPage.Navigation.PushAsync(new LunchDetailPage(lunch));
        }
    }
}
