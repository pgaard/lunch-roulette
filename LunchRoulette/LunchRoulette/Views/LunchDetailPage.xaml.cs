using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LunchRoulette.Models;
using LunchRoulette.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LunchRoulette.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LunchDetailPage : ContentPage
    {        
        public Lunch Lunch
        {
            get { return BindingContext as Lunch; }
            set { BindingContext = value; }
        }

        public LunchDetailPage(Lunch lunch)
        {
            this.Lunch = lunch;
            InitializeComponent();
        }

        private async void Save_OnClicked(object sender, EventArgs e)
        {
            var lunchService = new LunchService();
            await lunchService.Update(this.Lunch);

            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private const double StepValue = 0.25;

        private void Slider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / StepValue);

            Lunch.Rating = newStep * StepValue;        
        }
    }
}
