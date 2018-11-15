using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WorkStudy.Services
{
    public class Utilities
    {
        public Utilities()
        {
           
        }
        public static int StudyId { get; set; }
        public static int OperatorId { get; set; }
        private static ObservableCollection<string> comments = new ObservableCollection<string>();
        public static ObservableCollection<string> Comments
        {
            get { return comments; }
            set { comments = value; }
        }


        private static ObservableCollection<string> activities = new ObservableCollection<string>();
        public static ObservableCollection<string> Activities
        {
            get { return activities; }
            set { activities = value; }
        }
        public static async void Navigate(ContentPage page)
        {
            await Task.Delay(1000);
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
        }
    }
}
