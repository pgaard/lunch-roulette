using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace LunchRoulette.Views
{
    public partial class TabbedHomePage : TabbedPage
    {
        public TabbedHomePage()
        {
            InitializeComponent();
            this.Children.Add(new LandingPage());
            this.Children.Add(new LunchListPage());
        }
    }
}
