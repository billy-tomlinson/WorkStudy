using Xamarin.Forms;

namespace WorkStudy
{
    public partial class MainPage : ContentPage
    {
        void Submit_Clicked(object sender, System.EventArgs e)
        {
            var vm = BindingContext as MainListView;
            vm?.UpdateStudyNumber();
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
    }
}
