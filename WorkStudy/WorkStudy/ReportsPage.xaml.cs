
using Xamarin.Forms;

namespace WorkStudy
{
    public partial class ReportsPage : ContentPage
    {
        public ReportsPage()
        {
            InitializeComponent();
            chart.Source = ImageSource.FromFile("chart.png");
            NavigationPage.SetHasNavigationBar(this, false);
        }

        void Submit_Clicked(object sender, System.EventArgs e)
        {
            chartView.IsVisible = true;
        }

        void Ok_Clicked(object sender, System.EventArgs e)
        {
            chartView.IsVisible = false;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }


}
