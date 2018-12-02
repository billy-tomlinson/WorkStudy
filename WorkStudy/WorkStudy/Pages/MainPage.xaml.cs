using WorkStudy.Services;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy
{
    public partial class MainPage : ContentPage
    {
        public MainPage(bool isInvalid = false)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new MainPageViewModel(isInvalid);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnDisappearing()
        {
            if(!Utilities.AllObservationsTaken)
                Navigation.PushAsync(new MainPage(true));
            base.OnDisappearing();
        }
    }
}
