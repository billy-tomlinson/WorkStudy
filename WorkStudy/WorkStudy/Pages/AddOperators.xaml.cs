using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy
{
    public partial class AddOperators : ContentPage
    {

        public AddOperators()
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
        }

    }
}

