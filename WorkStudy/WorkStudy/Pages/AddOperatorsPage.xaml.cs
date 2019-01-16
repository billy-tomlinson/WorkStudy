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
                        || !Utilities.OperatorPageHasUpdatedOperatorChanges)
                {
                    Utilities.OperatorPageHasUpdatedActivityChanges = true;
                    Utilities.OperatorPageHasUpdatedOperatorChanges = true;

                    Utilities.UpdateTableFlags();

                    var viewModel = new AddOperatorsViewModel
                    {
                        RunningTotalsVisible = false
                    };
                    BindingContext = viewModel;
                }
            }

            //var viewModel = new AddOperatorsViewModel
            //{
            //    RunningTotalsVisible = false
            //};
            //BindingContext = viewModel;
            Utilities.ClearNavigation();
            base.OnAppearing();
        }
    }
}

