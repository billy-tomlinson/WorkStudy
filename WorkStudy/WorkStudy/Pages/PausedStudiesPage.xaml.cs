using WorkStudy.Services;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class PausedStudiesPage : ContentPage
    {
        public PausedStudiesPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            listOfStudies.BindingContext = new ExistingStudiesViewModel(false);
        }

        //public PausedStudiesPage(bool completed)
        //{
        //    InitializeComponent();
        //    NavigationPage.SetHasNavigationBar(this, false);
        //    listOfStudies.BindingContext = new ExistingStudiesViewModel(completed);

        //}

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Utilities.ClearNavigation();
        }
    }
}
