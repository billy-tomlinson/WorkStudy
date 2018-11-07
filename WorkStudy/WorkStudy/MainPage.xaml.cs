using Xamarin.Forms;

namespace WorkStudy
{
    public partial class MainPage : ContentPage
    {
        void Submit_Clicked(object sender, System.EventArgs e)
        {
            var vm = BindingContext as MainListView;

            Navigate();
            vm?.UpdateStudyNumber();
        }

        void End_Clicked(object sender, System.EventArgs e)
        {

            NavigateToReports();
        }

        public MainPage()
        {
            InitializeComponent();
        }

        private void ListViewItem_Tabbed(object sender, ItemTappedEventArgs e)
        {
            var product = e.Item as Product;
            var vm = BindingContext as MainListView;
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
    }
}
