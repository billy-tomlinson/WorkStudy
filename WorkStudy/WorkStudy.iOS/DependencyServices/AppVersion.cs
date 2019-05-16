using Foundation;
using WorkStudy.iOS.DependencyServices;
using WorkStudy.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersion))]
namespace WorkStudy.iOS.DependencyServices
{
    public class AppVersion : IAppVersion
    {
        public string GetVersion()
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString();
        }
        public string GetBuild()
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();
        }
    }
}