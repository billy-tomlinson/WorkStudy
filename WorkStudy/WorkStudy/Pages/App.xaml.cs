using WorkStudy.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using WorkStudy.Services;

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
            NavigationPage.SetHasNavigationBar(this, false);
            DatabasePath = databasePath;
            Utilities.Connection = DatabasePath;
            CallMain();
            //MainPage = new NavigationPage(new AlarmPage());
        }

        public static NavigationPage NavigationPage { get; private set; }
        public static RootPage RootPage;
        public static bool MenuIsPresented
        {
            get
            {
                return RootPage.IsPresented;
            }
            set
            {
                RootPage.IsPresented = value;
            }
        }

        private void CallMain()
        {
            var menuPage = new MenuPage(){ Title = "Main Page" };
            NavigationPage = new NavigationPage(new  WelcomePage());
            RootPage = new RootPage();
            RootPage.Master = menuPage;
            RootPage.Detail = NavigationPage;
            MainPage = RootPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start("android=4b7dad8c-e515-413d-9a8e-9e060c86a511;", 
                            typeof(Analytics), typeof(Crashes));
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
