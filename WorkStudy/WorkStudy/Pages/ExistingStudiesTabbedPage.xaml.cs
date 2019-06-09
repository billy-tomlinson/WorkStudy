
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class ExistingStudiesTabbedPage : TabbedPage
    {
        public ExistingStudiesTabbedPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetBackButtonTitle(this, "");
            this.BackgroundColor = Color.FromHex(Utilities.RASBackGroundColour);
            this.BarBackgroundColor = Color.FromHex(Utilities.RASBackGroundColour);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Utilities.ClearNavigation();
        }
    }
}
