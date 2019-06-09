using WorkStudy.Services;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class AddOperatorsPage : ContentPage
    {

        public AddOperatorsPage()
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
                if (!Utilities.OperatorPageHasUpdatedActivityChanges
                        || !Utilities.OperatorPageHasUpdatedOperatorChanges
                        || !Utilities.OperatorPageHasUpdatedActivitySampleChanges)
                {
                    Utilities.OperatorPageHasUpdatedActivityChanges = true;
                    Utilities.OperatorPageHasUpdatedOperatorChanges = true;
                    Utilities.OperatorPageHasUpdatedActivitySampleChanges = true;

                    Utilities.UpdateTableFlags();

                    var viewModel = new AddOperatorsViewModel
                    {
                        RunningTotalsVisible = false
                    };
                    BindingContext = viewModel;
                }
            }

            Utilities.ClearNavigation();
            base.OnAppearing();
        }
    }
}

