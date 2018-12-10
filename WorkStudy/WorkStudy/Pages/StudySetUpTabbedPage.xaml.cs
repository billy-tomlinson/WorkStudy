
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class StudySetUpTabbedPage : TabbedPage
    {
        public StudySetUpTabbedPage()
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
