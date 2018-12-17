using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorkStudy.Custom;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class EditActivitiesViewModel : BaseViewModel
    {
        public Command SaveActivities { get; set; }
        public Command UnMergeActivities { get; set; }
        public Command CancelActivities { get; set; }
        public Command ActivitySelected { get; set; }

        public EditActivitiesViewModel(string conn): base(conn)
        {
            ConstructorSetUp();
        }

        public EditActivitiesViewModel()
        {
            ConstructorSetUp();
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
            activity.Colour = Utilities.UnClicked.GetHexString().Equals(activity.Colour.GetHexString()) 
                ? Utilities.Clicked : Utilities.UnClicked;
            list.RemoveAll(_ => _.Id == sender);
            list.Add(activity);
            Activities = ConvertListToObservable(list);
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

            var list1 = new List<Activity>(obsCollection);

            Activities = ConvertListToObservable(list1);
            return Utilities.BuildGroupOfActivities(Activities);
        }

        public void SaveActivityDetails()
        {
            if (mergedActivities.Count < 2) return;

            var countRated = mergedActivities.Any(x => x.Rated);
            var countUnrated = mergedActivities.Any(x => !x.Rated);

            if(countRated && countUnrated)
            {
                ValidationText = "Cannot merge rated and unrated activities together.";
                Opacity = 0.2;
                RefreshActivities();
                IsInvalid = true;
                return;
            }

            var parentActivity = new Activity();
            var returnId = ActivityRepo.SaveItem(parentActivity);
            parentActivity = ActivityRepo.GetItem(returnId);

            for (var i = 0; i < MergedActivities.Count; i++)
            {
                var existingObs = ObservationRepo.GetItems().Where(cw => cw.AliasActivityId == MergedActivities[i].Id);
                foreach (var item in existingObs)
                {
                    item.AliasActivityId = returnId;
                    ObservationRepo.SaveItem(item);
                }

                var merged = MergedActivities[i];
                MergedActivityRepo.SaveItem(new Model.MergedActivities() { ActivityId = parentActivity.Id, MergedActivityId = MergedActivities[i].Id });
                merged.IsEnabled = false;

                ActivityRepo.SaveItem(merged);

                parentActivity.Name = parentActivity.Name + " " + merged.Name;
                parentActivity.IsEnabled = true;
                parentActivity.Rated = countRated;

                ActivityRepo.SaveItem(parentActivity);
                parentActivity.Activities.Add(merged);
            }

            RefreshActivities();
        }

        public void UnMergeActivitiesEvent()
        {
            if (mergedActivities.Count != 1)
            {
                ValidationText = "Please select one activity only to un-merge";
                Opacity = 0.2;
                RefreshActivities();
                IsInvalid = true;
                return;
            }

            var mergedActivity = mergedActivities[0];

            var inUse = ObservationRepo.GetItems().Any(cw => cw.ActivityId == mergedActivity.Id);

            if (inUse)
            {
                ValidationText = "Activity cannot be un-merged. It has been used in the study";
                Opacity = 0.2;
                RefreshActivities();
                IsInvalid = true;
                return;
            }

            var mergedItems = MergedActivityRepo.GetItems()
                                                .Where(x => x.ActivityId == mergedActivity.Id)
                                                .ToList();

            foreach (var item in mergedItems)
            {
                var activity = ActivityRepo.GetItem(item.MergedActivityId);
                activity.IsEnabled = true;
                ActivityRepo.SaveItem(activity);

                activity = ActivityRepo.GetItem(item.ActivityId);
                activity.IsEnabled = false;
                ActivityRepo.SaveItem(activity);

                MergedActivityRepo.DeleteItem(item);
            }

            RefreshActivities();
        }

        private void RefreshActivities()
        {
            MergedActivities = new List<Activity>();
            Activities = Get_All_Enabled_Activities_WithChildren();
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
                item.Colour = Utilities.UnClicked;
                list1.Add(item);
            }

            Activities = ConvertListToObservable(list1);
            GroupActivities = Utilities.BuildGroupOfActivities(Activities);        
        }

        private void ConstructorSetUp()
        {
            SaveActivities = new Command(SaveActivityDetails);
            UnMergeActivities = new Command(UnMergeActivitiesEvent);
            CancelActivities = new Command(CancelActivityDetails);
            ActivitySelected = new Command(ActivitySelectedEvent);

            Activities = Get_All_Enabled_Activities_WithChildren();
            GroupActivities = ChangeButtonColourOnLoad();
            MergedActivities = new List<Activity>();
            InvalidText = $"There are no activities to merge for study {Utilities.StudyId}";
            IsPageVisible = (Utilities.StudyId > 0 && !Utilities.IsCompleted && Activities.Count > 0);
        }
    }
}
