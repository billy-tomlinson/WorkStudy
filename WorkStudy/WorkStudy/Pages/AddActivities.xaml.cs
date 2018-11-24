
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
            //validationView.BindingContext = new AddActivitiesViewModel();

        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
