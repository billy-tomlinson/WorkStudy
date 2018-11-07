using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkStudy
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WelcomePage : ContentPage
	{
		public WelcomePage ()
		{
			InitializeComponent ();
		    Navigate();
        }

	    async void Navigate()
	    {
	        await System.Threading.Tasks.Task.Delay(2000);
	        await Navigation.PushAsync(new AddOperators());
	    }
    }
}