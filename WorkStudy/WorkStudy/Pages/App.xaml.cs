using WorkStudy.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WorkStudy
{
    public partial class App : Application
    {
        public static string DatabasePath = string.Empty;
        public App(){ }

        public App(string databasePath)
        {
            InitializeComponent();

            DatabasePath = databasePath;

            MainPage = new MasterDetailPage()
            {
                Master = new MasterPage1() { Title = "Main Page" },
                Detail = new NavigationPage(new AddOperators())
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
