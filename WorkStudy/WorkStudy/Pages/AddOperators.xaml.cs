using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy
{
    public partial class AddOperators : ContentPage
    {

        AddOperatorsViewModel viewModel;

        public AddOperators()
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

