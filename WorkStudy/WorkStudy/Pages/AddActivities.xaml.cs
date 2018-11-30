
using WorkStudy.Services;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy
{
    public partial class AddActivities : ContentPage
    {
        public AddActivities()
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
