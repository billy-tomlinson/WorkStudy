
using TimeStudy.Services;
using Xamarin.Forms;

namespace TimeStudy.Pages
{
    public partial class ExistingStudiesTabbedPage : TabbedPage
    {
        public ExistingStudiesTabbedPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetBackButtonTitle(this, "");

            this.BackgroundColor = Color.FromHex(Utilities.TimeStudyBackGroundColour);
            this.BarBackgroundColor = Color.FromHex(Utilities.TimeStudyTabBarBackgroundColour);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Utilities.ClearNavigation();
        }
    }
}
