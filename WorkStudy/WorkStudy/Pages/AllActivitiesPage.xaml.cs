using WorkStudy.Services;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class AllActivitiesPage : ContentPage
    {
        public AllActivitiesPage()
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
            var viewModel = new AllActivitiesViewModel();

            BindingContext = viewModel;
            Utilities.ClearNavigation();
            base.OnAppearing();

        }
    }
}
