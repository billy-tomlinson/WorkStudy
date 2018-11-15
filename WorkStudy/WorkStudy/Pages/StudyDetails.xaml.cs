
using Xamarin.Forms;

namespace WorkStudy
{
    public partial class StudyDetails : ContentPage
    {
        public StudyDetails()
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
