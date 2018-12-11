using WorkStudy.Services;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class CompletedStudiesPage : ContentPage
    {
        public CompletedStudiesPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            listOfStudies.BindingContext = new ExistingStudiesViewModel(true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Utilities.ClearNavigation();
        }
    }
}
