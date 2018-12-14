using WorkStudy.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkStudy.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WelcomePage : ContentPage
	{
		public WelcomePage ()
		{
			InitializeComponent ();
            timeStudy.Source = ImageSource.FromFile("stopwatch.png");
            NavigationPage.SetHasNavigationBar(this, false);
		    Navigate();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Utilities.ClearNavigation();
        }

	    async void Navigate()
	    {
	        await System.Threading.Tasks.Task.Delay(3000);
	        await Navigation.PushAsync(new StudyMenuPage());
	    }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}