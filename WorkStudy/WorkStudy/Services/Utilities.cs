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


        private static ObservableCollection<string> activities = new ObservableCollection<string>();
        public static ObservableCollection<string> Activities
        {
            get { return activities; }
            set { activities = value; }
        }

        private static ObservableCollection<Observation> observations = new ObservableCollection<Observation>();
        public static ObservableCollection<Observation> Observations
        {
            get { return observations; }
            set { observations = value; }
        }

        public static async void Navigate(ContentPage page)
        {
            await Task.Delay(1000);
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
        }
    }
}
