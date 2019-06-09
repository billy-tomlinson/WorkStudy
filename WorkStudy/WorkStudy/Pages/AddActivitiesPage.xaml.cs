using WorkStudy.Custom;
using WorkStudy.Services;
using WorkStudy.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace WorkStudy.Pages
{
    public partial class AddActivitiesPage : ContentPage
    {
        public AddActivitiesPage()
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
            if (Utilities.ActivityTableUpdated || Utilities.OperatorTableUpdated || Utilities.ObservationTableUpdated)
            {
                if (!Utilities.ActivityPageHasUpdatedActivityChanges 
                        || !Utilities.ActivityPageHasUpdatedOperatorChanges
                        || !Utilities.ActivityPageHasUpdatedObservationChanges)
                {
                    Utilities.ActivityPageHasUpdatedActivityChanges = true;
                    Utilities.ActivityPageHasUpdatedOperatorChanges = true;
                    Utilities.ActivityPageHasUpdatedObservationChanges = true;

                    Utilities.UpdateTableFlags();

                    var viewModel = new AddActivitiesViewModel
                    {
                        CommentsVisible = false
                    };
                    BindingContext = viewModel;
                }
            }

            Utilities.ClearNavigation();
            base.OnAppearing();
           
        }

        private static void UpdateTableFlags()
        {
            if (Utilities.MainPageHasUpdatedActivityChanges
                                    && Utilities.ActivityPageHasUpdatedActivityChanges)
                Utilities.ActivityTableUpdated = false;

            if (Utilities.MainPageHasUpdatedOperatorChanges
                && Utilities.ActivityPageHasUpdatedOperatorChanges)
                Utilities.OperatorTableUpdated = false;

            if (Utilities.MainPageHasUpdatedObservationChanges
                && Utilities.ActivityPageHasUpdatedObservationChanges)
                Utilities.ObservationTableUpdated = false;
        }
    }
}
