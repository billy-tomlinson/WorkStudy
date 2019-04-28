using System.Linq;
using WorkStudy.Model;
using WorkStudy.Services;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            ConstructorSetUp();
        }

        private void ConstructorSetUp()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, "");
            BindingContext = new MainPageViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            var viewModel1 = new MainPageViewModel();
            List.ItemAppearing += (sender, e) =>
            {
                if(Utilities.SetUpForNextObservationRound == true)
                {
                    var observations = viewModel1.OperatorObservations.OrderBy(x => x.Id).FirstOrDefault();
                    List.ScrollTo(observations, ScrollToPosition.End, true);
                    Utilities.SetUpForNextObservationRound = false;
                }
            };

            AlarmNotificationService.CheckIfAlarmHasExpiredWhilstInBackgroundOrAlarmOff();

            if (Utilities.ActivityTableUpdated || Utilities.OperatorTableUpdated || Utilities.ObservationTableUpdated)
            {
                if(!Utilities.MainPageHasUpdatedActivityChanges 
                    || !Utilities.MainPageHasUpdatedOperatorChanges
                    || !Utilities.MainPageHasUpdatedObservationChanges)
                {
                    Utilities.MainPageHasUpdatedActivityChanges = true;
                    Utilities.MainPageHasUpdatedOperatorChanges = true;
                    Utilities.MainPageHasUpdatedObservationChanges = true;

                    Utilities.UpdateTableFlags();

                    var viewModel = new MainPageViewModel
                    {
                        RatingsVisible = false,
                        ActivitiesVisible = false,
                    };
                    BindingContext = viewModel;
                }
            }

            Utilities.ClearNavigation();
            base.OnAppearing();
        }
    }
}
