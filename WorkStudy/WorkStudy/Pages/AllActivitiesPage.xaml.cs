using WorkStudy.Services;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class AllActivitiesPage : ContentPage
    {
        public AllActivitiesPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, "");
            this.BackgroundColor = Color.FromHex(Utilities.RASBackGroundColour);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            if (Utilities.ActivityTableUpdated)
            {
                if (!Utilities.AllActivitiesPageHasUpdatedActivityChanges)
                {
                    Utilities.AllActivitiesPageHasUpdatedActivityChanges = true;

                    Utilities.UpdateTableFlags();

                    var viewModel = new AllActivitiesViewModel();

                    BindingContext = viewModel;
                }
            }

            Utilities.ClearNavigation();
            base.OnAppearing();

        }
    }
}
