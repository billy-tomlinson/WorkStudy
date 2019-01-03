
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.IO;
using Android.Views;
using System.Threading.Tasks;
using System;
using Android.Content;
using WorkStudy.Pages;
using WorkStudy.Services;
//using Toasts.Forms.Plugin.Sample;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Plugin.Toasts;

namespace WorkStudy.Droid
{
    [Activity(Label = "Toasts.Forms.Plugin.Sample.DroidAppCompat", Theme = "@style/MyTheme", MainLauncher = true, Icon = "@mipmap/icon", LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
    //[Activity(Label = "WorkStudy", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //public class MainActivity : FormsAppCompatActivity
    //{
    //    protected override void OnCreate(Bundle bundle)
    //    {
    //        base.OnCreate(bundle);

    //        ToolbarResource = Resource.Layout.Tabbar;
    //        TabLayoutResource = Resource.Layout.Toolbar;

    //        Xamarin.Forms.Forms.Init(this, bundle);

    //        DependencyService.Register<ToastNotification>();
    //        ToastNotification.Init(this, new PlatformOptions() { SmallIconDrawable = Android.Resource.Drawable.IcDialogInfo });

    //        LoadApplication(new App());
    //    }
    //}
    //[Activity(Label = "WorkStudy", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            this.Window.AddFlags(WindowManagerFlags.Fullscreen); // hide the status bar

            //SetContentView(Resource.Layout.Main);

            int uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)SystemUiFlags.LowProfile;
            uiOptions |= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility =
             (StatusBarVisibility)uiOptions;

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            string dbName = "WorkStudy4.db3";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string dbPath = Path.Combine(folderPath, dbName);

            DependencyService.Register<ToastNotification>();
            ToastNotification.Init(this, new PlatformOptions() { SmallIconDrawable = Android.Resource.Drawable.IcDialogInfo });

            LoadApplication(new WorkStudy.App(dbPath));
            //LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            NotificationClickedOn(intent);
        }


        protected override void OnResume()
        {
            base.OnResume();
            //Utilities.Navigate(new StudySetUpTabbedPage());

        }

        void NotificationClickedOn(Intent intent)
        {
            if (intent.Action == "ASushiNotification" && intent.HasExtra("MessageFromSushiHangover"))
            {
                /// Do something now that you know the user clicked on the notification...

                //var notificationMessage = intent.Extras.GetString("MessageFromSushiHangover");
                //var winnerToast = Toast.MakeText(this, $"{notificationMessage}.\n\n🍣 Please send 2 BitCoins to SushiHangover to process your winning ticket! 🍣", ToastLength.Long);
                //winnerToast.SetGravity(Android.Views.GravityFlags.Center, 0, 0);
                //winnerToast.Show();
            }
        }
    }
}