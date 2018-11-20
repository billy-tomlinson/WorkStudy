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

            Activities = GetActivitiesWithChildren();
            GroupActivities = ChangeButtonColourOnLoad();
            MergedActivities = new List<Activity>();
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
            var list = new List<Activity>(obsCollection);
            var list1 = new List<Activity>(obsCollection);

            Activities = ConvertListToObservable(list1);
            return Utilities.BuildGroupOfActivities(Activities);
        }

        void SaveActivityDetails()
        {
            if (mergedActivities.Count == 0) return;

            var operators = OperatorRepo.GetItems().ToList();

            var operatorActivities = OperatorActivityRepo.GetItems().ToList();

            var parentActivity = MergedActivities[0];
            for (int i = 1; i < MergedActivities.Count; i++)
            {
                var merged = MergedActivities[i];
                merged.IsEnabled = false;
                parentActivity.Name = parentActivity.Name + "/" + merged.Name;
                parentActivity.Activities.Add(merged);
                ActivityRepo.SaveItem(merged);

                //use upddate with children !!
                foreach (var item in operators)
                {
                    foreach (var opAct in operatorActivities)
                    {
                        if(opAct.ActivityId == merged.Id)
                        {
                            opAct.ActivityId = parentActivity.Id;
                            OperatorActivityRepo.SaveItem(opAct);
                        }
                    }
                }
            }

            MergedActivities = new List<Activity>();

            ActivityRepo.DatabaseConnection.UpdateWithChildren(parentActivity);
            Activities = GetActivitiesWithChildren();
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

            Activities = ConvertListToObservable(list1);
            GroupActivities = Utilities.BuildGroupOfActivities(Activities);        
        }
    }
}
