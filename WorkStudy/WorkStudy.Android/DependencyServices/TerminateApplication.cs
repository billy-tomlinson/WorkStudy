using Android.App;
using WorkStudy.Droid.DependencyServices;
using WorkStudy.Services;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(TerminateApplication))]
namespace WorkStudy.Droid.DependencyServices
{
    public class TerminateApplication : ITerminateApplication
    {
        public void CLoseApplication()
        {
            var activity = (Activity)Application.Context;
            activity.FinishAffinity();
        }
    }
}