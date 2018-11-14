using System;
using WorkStudy.Model;
using WorkStudy.Services;

namespace WorkStudy.ViewModels
{
    public class StudyDetailsViewModel : BaseViewModel
    {
        readonly IBaseRepository<ActivitySampleStudy> sampleRepo;

        public StudyDetailsViewModel()
        {
            sampleRepo = new BaseRepository<ActivitySampleStudy>(App.DatabasePath);
        }

        override public void SubmitDetailsAndNavigate()
        {
            ActivitySampleStudy sampleStudy = new ActivitySampleStudy
            {
                Name = Name,
                Date = Date,
                IsRated = IsRated,
                StudiedBy = StudiedBy,
                Department = Department,
                StudyNumber = StudyNumber
            };

            Utilities.StudyId = sampleRepo.SaveItem(sampleStudy);

            Utilities.Navigate(new AddActivities());
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

        string department;
        public string Department
        {
            get { return department; }
            set
            {
                department = value;
                OnPropertyChanged();
            }
        }

        string studiedBy;
        public string StudiedBy
        {
            get { return studiedBy; }
            set
            {
                studiedBy = value;
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

        int studyNumber;
        public int StudyNumber
        {
            get { return studyNumber; }
            set
            {
                studyNumber = value;
                OnPropertyChanged();
            }
        }

        bool isRated;
        public bool IsRated
        {
            get { return isRated; }
            set
            {
                isRated = value;
                OnPropertyChanged();
            }
        }

        bool completed;
        public bool Completed
        {
            get { return completed; }
            set
            {
                completed = value;
                OnPropertyChanged();
            }
        }
    }
}
