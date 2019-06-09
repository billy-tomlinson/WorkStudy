using TimeStudy.Services;
using Xamarin.Forms;

namespace TimeStudy.Pages
{
    public partial class TimeStudyMainPageTabbedPage : TabbedPage
    {
        public TimeStudyMainPageTabbedPage(bool setUp = false)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetBackButtonTitle(this, "");

            this.BackgroundColor = Color.FromHex(Utilities.TimeStudyBackGroundColour);
            this.BarBackgroundColor = Color.FromHex(Utilities.TimeStudyTabBarBackgroundColour);

            if(setUp)
                this.CurrentPage = this.Children[1];

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

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
        }
    }
}
