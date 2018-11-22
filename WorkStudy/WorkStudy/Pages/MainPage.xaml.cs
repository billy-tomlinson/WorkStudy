using Xamarin.Forms;
using WorkStudy.Pages;

namespace WorkStudy
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
