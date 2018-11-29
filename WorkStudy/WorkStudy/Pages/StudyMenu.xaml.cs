using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class StudyMenu : ContentPage
    {
        public StudyMenu()
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
