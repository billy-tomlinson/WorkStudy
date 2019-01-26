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
            //MainPage = new NavigationPage(new ReportsPage());
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
            var menuPage = new MenuPage(){ Title = "Main Page" , Icon="hamburger.png"  };
            NavigationPage = new NavigationPage(new WelcomePage());
            RootPage = new RootPage();
            RootPage.Master = menuPage;
            RootPage.Detail = NavigationPage;
            MainPage = RootPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts

            AppCenter.Start("ios=e79293ae-e7fe-4968-b897-6df32f2bf00a;" +
                  "android=4b7dad8c-e515-413d-9a8e-9e060c86a511;",
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



//namespace Toasts.Forms.Plugin.Sample
//{
//    public partial class App : Application
//    {
//        public App()
//        {
//            Button showToast = new Button { Text = "Show Toast" };

//            showToast.Clicked += (s, e) =>
//            {

//                ShowToast(new NotificationOptions()
//                {
//                    Title = "The Title Line",
//                    Description = "The Description Content",
//                    IsClickable = true,
//                    WindowsOptions = new WindowsOptions() { LogoUri = "icon.png" },
//                    ClearFromHistory = false,
//                    AllowTapInNotificationCenter = false,
//                    AndroidOptions = new AndroidOptions()
//                    {
//                        HexColor = "#F99D1C",
//                        ForceOpenAppOnNotificationTap = true
//                    }
//                });
//            };

//            // The root page of your application
//            MainPage = new ContentPage
//            {
//                Content = new StackLayout
//                {
//                    VerticalOptions = LayoutOptions.Center,
//                    Children = {
//                        showToast
//                    }
//                }
//            };

//        }

//        void ShowToast(INotificationOptions options)
//        {
//            var notificator = DependencyService.Get<IToastNotificator>();

//            // await notificator.Notify(options);

//            notificator.Notify((INotificationResult result) =>
//            {
//                System.Diagnostics.Debug.WriteLine("Notification [" + result.Id + "] Result Action: " + result.Action);
//            }, options);
//        }

//    }
//}

