using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class StudyMenuPage : ContentPage
    {
        public StudyMenuPage()
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
