using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SQLiteNetExtensions.Extensions;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class EditActivitiesViewModel : BaseViewModel
    {
        public Command SaveActivities { get; set; }
        public Command CancelActivities { get; set; }
        public Command ActivitySelected { get; set; }

        public EditActivitiesViewModel()
        {
            SaveActivities = new Command(SaveActivityDetails);
            CancelActivities = new Command(CancelActivityDetails);
            ActivitySelected = new Command(ActivitySelectedEvent);

            Activities = new ObservableCollection<Activity>(ActivityRepo.DatabaseConnection.GetAllWithChildren<Activity>().OrderBy(x => x.Name).Where(x => x.IsEnabled == true));
            GroupActivities = ChangeButtonColourOnLoad();
            MergedActivities = new List<Activity>();
        }

        static ObservableCollection<MultipleActivities> _groupActivities;
        public ObservableCollection<MultipleActivities> GroupActivities
        {
            get => _groupActivities;
            set
            {
                _groupActivities = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Activity> activities;
        public ObservableCollection<Activity> Activities
        {
            get => activities;
            set
            {
                activities = value;
                OnPropertyChanged();
            }
        }

        private List<Activity> mergedActivities;
        public List<Activity> MergedActivities
        {
            get => mergedActivities;
            set
            {
                mergedActivities = value;
                OnPropertyChanged();
            }
        }

        void ActivitySelectedEvent(object sender)
        {

            var value = (int)sender;
            ChangeButtonColour(value);

        }

        private void ChangeButtonColour(int sender)
        {
            IEnumerable<Activity> obsCollection = Activities;
            var list = new List<Activity>(obsCollection);
            var activity = list.Find(_ => _.Id == sender);
            activity.Colour = System.Drawing.Color.Aquamarine.ToArgb().Equals(activity.Colour.ToArgb()) ? System.Drawing.Color.BlueViolet : System.Drawing.Color.Aquamarine;
            list.RemoveAll(_ => _.Id == sender);
            list.Add(activity);
            Activities = new ObservableCollection<Activity>(list.OrderBy(x => x.Name));
            GroupActivities = Utilities.BuildGroupOfActivities(Activities);

            var merged = MergedActivities.Find(_ => _.Id == sender);
            if (merged == null)
                MergedActivities.Add(activity);
            else
                MergedActivities.RemoveAll(_ => _.Id == sender);
        }

        private ObservableCollection<MultipleActivities> ChangeButtonColourOnLoad()
        {
            IEnumerable<Activity> obsCollection = Activities;
            var list = new List<Activity>(obsCollection);
            var list1 = new List<Activity>(obsCollection);

            Activities = new ObservableCollection<Activity>(list1.OrderBy(x => x.Name));
            return Utilities.BuildGroupOfActivities(Activities);
        }

        void SaveActivityDetails()
        {
            if (mergedActivities.Count == 0) return;

            var parentActivity = MergedActivities[0];
            for (int i = 1; i < MergedActivities.Count; i++)
            {
                var merged = MergedActivities[i];
                merged.IsEnabled = false;
                parentActivity.Name = parentActivity.Name + "/" + merged.Name;
                parentActivity.Activities.Add(merged);
                ActivityRepo.SaveItem(merged);
            }

            MergedActivities = new List<Activity>();

            ActivityRepo.DatabaseConnection.UpdateWithChildren(parentActivity);
            Activities = new ObservableCollection<Activity>(ActivityRepo.DatabaseConnection.GetAllWithChildren<Activity>().OrderBy(x => x.Name).Where(x => x.IsEnabled == true));
            GroupActivities = Utilities.BuildGroupOfActivities(Activities);
        }

        void CancelActivityDetails()
        {
            IEnumerable<Activity> obsCollection = Activities;
            var list1 = new List<Activity>(obsCollection);
            var list = new List<Activity>(obsCollection);

            foreach (var item in list)
            {
                list1.RemoveAll(_ => _.Id == (int)item.Id);
                item.Colour = System.Drawing.Color.Aquamarine;
                list1.Add(item);
            }

            Activities = new ObservableCollection<Activity>(list1.OrderBy(x => x.Name).Where(x => x.IsEnabled == true));
            GroupActivities = Utilities.BuildGroupOfActivities(Activities);        
        }
    }
}
