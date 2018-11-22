using WorkStudy.Model;
using Xamarin.Forms;
using WorkStudy.ViewModels;
using WorkStudy.Pages;

namespace WorkStudy
{
    public partial class MainPage : ContentPage
    {
        static Operator _operator;
        static string Name;
        MainPageViewModel viewModel;

        void Submit_Clicked(object sender, System.EventArgs e)
        {
            
            var vm = BindingContext as MainPageViewModel;
            vm?.UpdateStudyNumber();
            Navigate();
        }
        void Rating_Clicked(object sender, System.EventArgs e)
        {
            var vm = BindingContext as MainPageViewModel;
            vm?.ShowOrHideOperators(_operator);
            ratingView.IsVisible = false;
        }
        void End_Clicked(object sender, System.EventArgs e)
        {

            NavigateToReports();
        }

        void Pause_Clicked(object sender, System.EventArgs e)
        {

            NavigateToStudyMenu();
        }

        public MainPage()
        {
            InitializeComponent();
            viewModel = BindingContext as MainPageViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
        }
        private void ListViewItem_Tabbed(object sender, ItemTappedEventArgs e)
        {
            _operator = e.Item as Operator;
            Name = _operator.Name;
            var vm = BindingContext as MainPageViewModel;
            activityView.IsVisible = true;
            vm?.ShowOrHideOperators(_operator);
        }
        async void Navigate()
        {
            await Navigation.PushModalAsync(new MainPage());
        }
        async void NavigateToReports()
        {
            await System.Threading.Tasks.Task.Delay(1000);
            await Navigation.PushModalAsync(new ReportsPage());
        }

        async void NavigateToStudyMenu()
        {
            await System.Threading.Tasks.Task.Delay(1000);
            await Navigation.PushModalAsync(new StudyMenu());
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
