using WorkStudy.Services;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class AddActivitiesPage : ContentPage
    {
        public AddActivitiesPage()
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
            var viewModel = new AddActivitiesViewModel
            {
                CommentsVisible = false
            };
            BindingContext = viewModel;
            Utilities.ClearNavigation();
            base.OnAppearing();
           
        }
    }
}
