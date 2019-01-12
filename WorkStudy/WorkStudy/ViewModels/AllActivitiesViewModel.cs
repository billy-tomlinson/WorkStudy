using System.Collections.Generic;
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
            Submit = new Command(AddHistoricActivities);

            IsPageVisible = true;
            ItemsCollection = Get_Previous_Rated_Activities();
        }

        static ObservableCollection<Activity> itemsCollection;
        public ObservableCollection<Activity> ItemsCollection
        {
            get => itemsCollection;
            set
            {
                itemsCollection = value;
                OnPropertyChanged();
            }
        }

        void AddHistoricActivities(object sender)
        {
            var sample = SampleRepo.GetItem(Utilities.StudyId);

            foreach (var item in ItemsCollection.Where(x => x.Selected))
            {
                item.ActivitySampleStudies = new List<ActivitySampleStudy>() { sample };
                ActivityRepo.InsertOrReplaceWithChildren(item);
            }
        }
    }
}
