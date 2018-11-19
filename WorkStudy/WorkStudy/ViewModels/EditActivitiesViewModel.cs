using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        readonly IBaseRepository<Activity> activitiesRepo;
        readonly IBaseRepository<MergedActivities> mergedActivityRepo;

        public EditActivitiesViewModel()
        {
            SaveActivities = new Command(SaveActivityDetails);
            CancelActivities = new Command(CancelActivityDetails);
            ActivitySelected = new Command(ActivitySelectedEvent);

            activitiesRepo = new BaseRepository<Activity>();
            mergedActivityRepo = new BaseRepository<MergedActivities>();

            Activities = new ObservableCollection<Activity>(activitiesRepo.DatabaseConnection.GetAllWithChildren<Activity>());
            GroupActivities = Utilities.BuildGroupOfActivities(Activities);
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
            list.RemoveAll(_ => _.Id == (int)sender);
            list.Add(activity);
            Activities = new ObservableCollection<Activity>(obsCollection);
            GroupActivities = Utilities.BuildGroupOfActivities(Activities);
        }

        void SaveActivityDetails()
        {

        }

        void CancelActivityDetails()
        {
            
        }
    }
}
