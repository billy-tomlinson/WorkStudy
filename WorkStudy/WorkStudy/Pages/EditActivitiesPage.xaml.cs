using WorkStudy.Services;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class EditActivitiesPage : ContentPage
    {
        public EditActivitiesPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, "");
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            if (Utilities.ActivityTableUpdated)
            {
                if (!Utilities.MergePageHasUpdatedActivityChanges)
                {
                    Utilities.MergePageHasUpdatedActivityChanges = true;

                    Utilities.UpdateTableFlags();

                    var viewModel = new EditActivitiesViewModel();

                    BindingContext = viewModel;
                }
            }

            //BindingContext = new EditActivitiesViewModel();
            base.OnAppearing();
            Utilities.ClearNavigation();
        }
    }
}
