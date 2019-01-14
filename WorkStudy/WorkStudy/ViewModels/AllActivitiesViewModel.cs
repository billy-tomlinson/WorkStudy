using System.Collections.ObjectModel;
using System.Linq;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class AllActivitiesViewModel : BaseViewModel
    {
        public Command Submit { get; set; }

        public AllActivitiesViewModel()
        {
            Submit = new Command(ActivitySelectedEvent);
            ItemsCollection = GetUnusedActivities();
        }

        private ObservableCollection<ActivityName> GetUnusedActivities()
        {
            var allActivities = Get_All_ActivityNames().Where(x => !x.IsMerge).ToList();

            var activityNames = allActivities.Select(x => x.Name).ToList();

            var activitiesInStudy = ActivityRepo.GetAllWithChildren().Where(x => x.StudyId == Utilities.StudyId);

            foreach (var item in activityNames)
            {
                foreach (var activity in activitiesInStudy)
                {
                    if (item == activity.ActivityName.Name)
                    {
                        var v = allActivities.FirstOrDefault(x => x.Name == item);
                        allActivities.Remove(v);
                    }
                }
            }

            return new ObservableCollection<ActivityName>(allActivities);
        }

        static ObservableCollection<ActivityName> itemsCollection;
        public ObservableCollection<ActivityName> ItemsCollection
        {
            get => itemsCollection;
            set
            {
                itemsCollection = value;
                OnPropertyChanged();
            }
        }

        void ActivitySelectedEvent(object sender)
        {
            //var sample = SampleRepo.GetItem(Utilities.StudyId);

            foreach (var item in ItemsCollection.Where(x => x.Selected))
            {
                var activity = new Activity
                {
                    ActivityName = item,
                    IsEnabled = true,
                    Rated = true
                };

                var returnID = SaveActivityDetails(activity);
            }

            var page = sender as ContentPage;
            var parentPage = page.Parent as TabbedPage;
            parentPage.CurrentPage = parentPage.Children[1];
        }
    }
}