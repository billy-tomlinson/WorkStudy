using Xamarin.Forms;

namespace WorkStudy
{
    public partial class MainPage : ContentPage
    {
        static Product product;
        static string Name;

        void Submit_Clicked(object sender, System.EventArgs e)
        {
            
            var vm = BindingContext as MainListView;
            vm?.UpdateStudyNumber();
            Navigate();
        }


        void Activity_Clicked(object sender, System.EventArgs e)
        {
            activityView.IsVisible = false;
            ratingView.IsVisible = true;
        }


        void Rating_Clicked(object sender, System.EventArgs e)
        {
            var vm = BindingContext as MainListView;
            vm?.ShoworHiddenProducts(product);
            ratingView.IsVisible = false;
        }

        void End_Clicked(object sender, System.EventArgs e)
        {

            NavigateToReports();
        }

        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void ListViewItem_Tabbed(object sender, ItemTappedEventArgs e)
        {
            product = e.Item as Product;
            Name = product.Title;
            var vm = BindingContext as MainListView;
            displayNameEntry.Text = Name;
            activityView.IsVisible = true;
            vm?.ShoworHiddenProducts(product);
        }

        async void Navigate()
        {
            await Navigation.PushAsync(new MainPage());
        }

        async void NavigateToReports()
        {
            await System.Threading.Tasks.Task.Delay(1000);
            await Navigation.PushAsync(new ReportsPage());
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
