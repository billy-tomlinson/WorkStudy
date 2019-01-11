using System.Threading;
using WorkStudy.Services;
using WorkStudy.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(TerminateApplication))]
namespace WorkStudy.iOS.DependencyServices
{
    public class TerminateApplication : ITerminateApplication
    {
        public void CloseApplication()
        {
            Thread.CurrentThread.Abort();
        }
    }
}