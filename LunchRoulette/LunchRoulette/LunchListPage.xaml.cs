using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunchRoulette.Models;
using Xamarin.Forms;

namespace LunchRoulette
{
    public partial class LunchListPage : ContentPage
    {
        public int Build { get; set; }
        public ObservableCollection<Lunch> Lunches;

        public LunchListPage()
        {
            this.Build = 1;
            InitializeComponent();
            this.Add_OnClicked(null, null);
        }

        private void Add_OnClicked(object sender, EventArgs e)
        {
            var lunches = new List<Lunch>
            {
                new Lunch {RestaurantName = "burger king", Date = new DateTime(2016, 12, 11)},
                new Lunch {RestaurantName = "naf naf", Date = new DateTime(2016, 12, 12)},
                new Lunch {RestaurantName = "leann chin", Date = new DateTime(2016, 12, 13)}
            };
            this.Lunches = new ObservableCollection<Lunch>(lunches);
            this.chowList.ItemsSource = this.Lunches;
        }
    }
}
