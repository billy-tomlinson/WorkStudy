using WorkStudy.Droid.DependencyServices;
using WorkStudy.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(TerminateApplication))]
namespace WorkStudy.Droid.DependencyServices
{
    public class TerminateApplication : ITerminateApplication
    {
        public void CloseApplication()
        {
            Java.Lang.JavaSystem.Exit(0);
        }
    }
}