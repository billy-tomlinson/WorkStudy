using System.Windows.Input;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class AboutPageViewModel : BaseViewModel
    {
        public ICommand ClickCommand => new Command<string>((url) =>
        {
            Device.OpenUri(new System.Uri(url));
        });
    }
}
