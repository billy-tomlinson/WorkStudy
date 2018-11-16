using System.Threading.Tasks;
using Xamarin.Forms;

namespace WorkStudy.Services
{
    public class Utilities
    {      
        public static int StudyId { get; set; }
        public static int OperatorId { get; set; }

        public static async void Navigate(ContentPage page)
        {
            await Task.Delay(1000);
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
        }
    }
}
