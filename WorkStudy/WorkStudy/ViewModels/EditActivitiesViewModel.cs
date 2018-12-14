using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SQLiteNetExtensions.Extensions;
using WorkStudy.Custom;
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
            var list = new List<Activity>(obsCollection);
            var list1 = new List<Activity>(obsCollection);

            Activities = ConvertListToObservable(list1);
            return Utilities.BuildGroupOfActivities(Activities);
        }

        public void SaveActivityDetails()
        {
            if (mergedActivities.Count < 2) return;

            var operators = OperatorRepo.GetAllWithChildren().ToList();

            var parentActivity = new Activity();
            var returnId = ActivityRepo.SaveItem(parentActivity);
            parentActivity = ActivityRepo.GetItem(returnId);

            for (var i = 0; i < MergedActivities.Count; i++)
            {
                var merged = MergedActivities[i];
                MergedActivityRepo.SaveItem(new Model.MergedActivities() { ActivityId = parentActivity.Id, MergedActivityId = MergedActivities[i].Id });
                merged.IsEnabled = false;

                parentActivity.Name = parentActivity.Name + " " + merged.Name;
                parentActivity.IsEnabled = true;
                parentActivity.Rated = true;

                ActivityRepo.SaveItem(parentActivity);
                parentActivity.Activities.Add(merged);

                ActivityRepo.SaveItem(merged);

                //foreach (var item in operators)
                //{
                //    for (int x = 0; x < item.Activities.Count; x++)
                //    {
                //        if (item.Activities[x].Id == MergedActivities[i].Id)
                //        {
                //            OperatorActivityRepo.SaveItem(new OperatorActivity { ActivityId = parentActivity.Id, OperatorId = item.Id });
                //        }
                //    }
                //}
            }

            MergedActivities = new List<Activity>();
            Activities = Get_Rated_Enabled_Activities_WithChildren();
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
            CancelActivities = new Command(CancelActivityDetails);
            ActivitySelected = new Command(ActivitySelectedEvent);

            Activities = Get_Rated_Enabled_Activities_WithChildren();
            GroupActivities = ChangeButtonColourOnLoad();
            MergedActivities = new List<Activity>();
            InvalidText = $"There are no activities to merge for study {Utilities.StudyId}";
            IsPageVisible = (Utilities.StudyId > 0 && !Utilities.IsCompleted && Activities.Count > 0);
        }

    }
}
