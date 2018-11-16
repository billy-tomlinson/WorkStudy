using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WorkStudy.Model;
using Xamarin.Forms;

namespace WorkStudy.Services
{
    public class Utilities
    {
        
        public static int StudyId { get; set; }
        public static int OperatorId { get; set; }

        private static ObservableCollection<string> operators = new ObservableCollection<string>();
        public static ObservableCollection<string> Operators
        {
            get { return operators; }
            set { operators = value; }
        }

        public static async void Navigate(ContentPage page)
        {
            await Task.Delay(1000);
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
        }
    }
}
