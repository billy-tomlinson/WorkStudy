using System;
using WorkStudy.Model;
using WorkStudy.Services;

namespace WorkStudy.ViewModels
{
    public class AddActivitiesViewModel : BaseViewModel
    {
        readonly IBaseRepository<Activity> activityRepo;
       
        public AddActivitiesViewModel()
        {
            activityRepo = new BaseRepository<Activity>();
        }

        string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged();
            }
        }

        bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged();
            }
        }

        DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }

        override public void SubmitDetailsAndNavigate()
        {
            Activity acivity = new Activity
            {
                Name = Name,
                Date = DateTime.Now
            };

            activityRepo.SaveItem(acivity);

            Utilities.Navigate(new AddOperators());
        }
    }
}
