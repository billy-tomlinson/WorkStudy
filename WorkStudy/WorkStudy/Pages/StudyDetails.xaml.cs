
using Xamarin.Forms;

namespace WorkStudy
{
    public partial class StudyDetails : ContentPage
    {
        public StudyDetails()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void Navigate()
        {
            await System.Threading.Tasks.Task.Delay(2000);
            await Navigation.PushAsync(new AddOperators());
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        void Submit_Clicked(object sender, System.EventArgs e)
        {
            Navigate();
        }
    }
}
