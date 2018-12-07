using WorkStudy.Services;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(bool isInvalid = false)
        {
            ConstructorSetUp(isInvalid);
        }

        public MainPage()
        {
            ConstructorSetUp(false);
        }

        private void ConstructorSetUp(bool isInvalid)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new MainPageViewModel(false);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            BindingContext = new MainPageViewModel();
            base.OnAppearing();
        }

        //protected override void OnDisappearing()
        //{
        //    if(!Utilities.AllObservationsTaken)
        //        Navigation.PushAsync(new MainPage(true));
        //    base.OnDisappearing();
        //}
    }
}
