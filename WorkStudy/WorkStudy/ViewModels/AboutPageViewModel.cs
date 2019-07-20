using System.Windows.Input;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class AboutPageViewModel : BaseViewModel
    {
    
        public ICommand ClickCommand => new Command<string>((url) =>
        {
            Device.OpenUri(new System.Uri(url));
        });

        public ICommand DemoCommand => new Command<string>((url) =>
        {
            string demoUrl = string.Empty;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    demoUrl = "https://www.youtube.com/watch?v=lLmF09eIxEo";
                    break;
                case Device.Android:
                    demoUrl = "https://www.youtube.com/watch?v=lLmF09eIxEo";
                    break;
            }

            Device.OpenUri(new System.Uri(demoUrl));
        });
    }
}
