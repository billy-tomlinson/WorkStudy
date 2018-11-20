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

        public static async void Navigate(ContentPage page)
        {
            await Task.Delay(1000);
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
        }

        public static ObservableCollection<MultipleActivities> BuildGroupOfActivities(ObservableCollection<Activity> activites)
        {
            int counter = 0;
            bool added = false;
            var multipleActivities = new MultipleActivities();
            var groupedActivities = new ObservableCollection<MultipleActivities>();

            for (int i = 0; i < activites.Count; i++)
            {
                var activity = activites[i];

                if (counter == 0)
                {
                    multipleActivities.ActivityOne = activity;
                    added = false;
                    counter++;
                }

                else if (counter == 1)
                {
                    multipleActivities.ActivityTwo = activity;
                    added = false;
                    counter++;
                }

                else if (counter == 2)
                {
                    multipleActivities.ActivityThree = activity;
                    groupedActivities.Add(multipleActivities);
                    added = true;
                    multipleActivities = new MultipleActivities();
                    counter = 0;
                }
            }

            if (!added) groupedActivities.Add(multipleActivities);

            return groupedActivities;
        }
    }
}
