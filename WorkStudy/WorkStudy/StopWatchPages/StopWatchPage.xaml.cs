
using WorkStudy.Services;
using Xamarin.Forms;
namespace StopWatch
{
    public partial class StopWatchPage : ContentPage
    {
        public StopWatchPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetBackButtonTitle(this, "");

            this.BackgroundColor = Color.FromHex(Utilities.StopWatchBackGroundColour);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        protected override void OnAppearing()
        {
            Utilities.ClearNavigation();
            base.OnAppearing();
        }
    }
}

