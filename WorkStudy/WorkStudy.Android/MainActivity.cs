﻿
using Android.App;
using Android.Runtime;
using Android.OS;
using System.IO;
using Android.Views;
using Android.Content;
using Plugin.Toasts;
using Android.Content.PM;

namespace WorkStudy.Droid
{
    [Activity(Label = "Work Study", Theme = "@style/MainTheme", MainLauncher = true, Icon = "@drawable/randomActivity", LaunchMode = Android.Content.PM.LaunchMode.SingleTop, ScreenOrientation = ScreenOrientation.Portrait)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
        
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            this.Window.AddFlags(WindowManagerFlags.Fullscreen); // hide the status bar

            int uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)SystemUiFlags.LowProfile;
            uiOptions |= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility =
             (StatusBarVisibility)uiOptions;

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            string dbName = "TyeManagementLtd.db3";
            string alarmDbName = "Alarm.db3";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string dbPath = Path.Combine(folderPath, dbName);
            string alarmDbPath = Path.Combine(folderPath, alarmDbName);

            ToastNotification.Init(this, new PlatformOptions() 
            { 
                SmallIconDrawable = Android.Resource.Drawable.IcDialogInfo 
            });

            LoadApplication(new WorkStudy.App(dbPath, alarmDbPath));
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

        void NotificationClickedOn(Intent intent){}
    }
}