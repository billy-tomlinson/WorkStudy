using WorkStudy.Services;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class AddOperatorsPage : ContentPage
    {

        public AddOperatorsPage()
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
            BindingContext = new AddOperatorsViewModel();
            base.OnAppearing();
            Utilities.ClearNavigation();
        }
    }
}

