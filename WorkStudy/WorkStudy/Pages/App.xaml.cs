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

            CallMain();
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
            var tabbedPage = new TestTabbedPage() { Title = "Tabbed Page" };
            var menuPage = new MenuPage(){ Title = "Main Page" };
            NavigationPage = new NavigationPage(new  TestTabbedPage());
            RootPage = new RootPage();
            RootPage.Master = menuPage;
            RootPage.Detail = NavigationPage;
            MainPage = RootPage;
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
