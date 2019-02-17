using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class StopWatch : ContentPage
    {
        public StopWatch()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetBackButtonTitle(this, "");
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
