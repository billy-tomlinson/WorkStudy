using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class HomeMenuPage : ContentPage
    {
        public HomeMenuPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, "");
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Utilities.ClearNavigation();
        }
    }
}
