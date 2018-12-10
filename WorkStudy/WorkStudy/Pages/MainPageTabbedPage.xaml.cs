using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class MainPageTabbedPage : TabbedPage
    {
        public MainPageTabbedPage()
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
