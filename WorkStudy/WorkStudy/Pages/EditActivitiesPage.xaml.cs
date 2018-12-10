using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class EditActivitiesPage : ContentPage
    {
        public EditActivitiesPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            BindingContext = new EditActivitiesViewModel();
            base.OnAppearing();
        }
    }
}
