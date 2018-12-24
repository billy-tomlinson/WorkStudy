
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class AlarmPage : ContentPage
    {
        public AlarmPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Utilities.ClearNavigation();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
