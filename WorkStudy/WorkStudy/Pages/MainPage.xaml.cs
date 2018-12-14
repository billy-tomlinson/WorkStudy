using WorkStudy.Services;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            ConstructorSetUp();
        }

        private void ConstructorSetUp()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new MainPageViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            var viewModel = new MainPageViewModel
            {
                RatingsVisible = false,
                ActivitiesVisible = false,
            };
            BindingContext = viewModel;
            Utilities.ClearNavigation();
            base.OnAppearing();
        }
    }
}
