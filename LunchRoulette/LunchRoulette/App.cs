using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

using LunchRoulette.Views;

namespace LunchRoulette
{
    public class App : Application
    {
        public App()
        {
            // https://coolors.co/98c1d9-6969b3-533a7b-4b244a-25171a
            MainPage = new NavigationPage(new TabbedHomePage()
            {
                BarBackgroundColor = Color.FromHex("6969B3")
            })
            {
                BarBackgroundColor = Color.FromHex("533A7B")
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
