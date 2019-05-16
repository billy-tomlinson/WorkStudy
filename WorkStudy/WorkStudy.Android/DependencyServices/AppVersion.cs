using Android.Content.PM;
using WorkStudy.Droid.DependencyServices;
using WorkStudy.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersion))]
namespace WorkStudy.Droid.DependencyServices
{
    public class AppVersion : IAppVersion
    {
        public string GetVersion()
        {
            var context = global::Android.App.Application.Context;

            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            return info.VersionName;
        }

        public string GetBuild()
        {
            var context = global::Android.App.Application.Context;
            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            return info.VersionCode.ToString();
        }
    }
}