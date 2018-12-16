using System;
using System.Linq;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class StudyDetailsViewModel : BaseViewModel
    {
        public StudyDetailsViewModel(string conn) : base(conn) { ConstructorSetUp(); }

        public StudyDetailsViewModel(){ ConstructorSetUp(); }

        override public void SubmitDetailsAndNavigate()
        {
            ValidateValues();

            if(!IsInvalid)
            {
                Utilities.StudyId = SampleRepo.SaveItem(SampleStudy);
                StudyNumber = Utilities.StudyId;
                CreateUnratedActivities();

                Utilities.RatedStudy = SampleStudy.IsRated;

                IsActive = false;
            }
        }

        bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                OnPropertyChanged();
            }
        }

        ActivitySampleStudy sampleStudy;
        public ActivitySampleStudy SampleStudy
        {
            get { return sampleStudy; }
            set
            {
                sampleStudy = value;
                OnPropertyChanged();
            }
        }

        private void ConstructorSetUp()
        {
            IsActive = true;

            Utilities.StudyId = 0;

            SampleStudy = new ActivitySampleStudy()
            {
                IsRated = true,
                Date = DateTime.Now,
                Time = DateTime.Now.TimeOfDay
            };

            IsPageVisible = true;

            int lastStudyId = 0;
            var studies = SampleRepo.GetItems()?.ToList();

            if(studies.Count > 0)
                lastStudyId = studies.OrderByDescending(x => x.Id)
                                        .FirstOrDefault().Id;
            
            lastStudyId = lastStudyId + 1;

            SampleStudy.StudyNumber = lastStudyId;
            CloseView = new Command(CloseValidationView);

        }

        private void ValidateValues()
        {
            ValidationText = "Please enter all study details";

            IsInvalid = true;
            Opacity = 0.2;

            if ((SampleStudy.Department != null &&  SampleStudy.Department?.Trim().Length > 0) &&
                (SampleStudy.Name != null && SampleStudy.Name?.Trim().Length > 0) &&
                (SampleStudy.StudiedBy != null && SampleStudy.StudiedBy?.Trim().Length > 0))
            {
                Opacity = 1;
                IsInvalid = false;
            }
                
        }

        public void CreateUnratedActivities()
        {
            var unrated1 = new Activity()
            {
                Name = "IDLE",
                IsEnabled = true,
                Rated = false,
                StudyId = Utilities.StudyId,
                DeleteIcon = string.Empty
            };

            var unrated2 = new Activity()
            {
                Name = "INACTIVE",
                IsEnabled = true,
                Rated = false,
                StudyId = Utilities.StudyId,
                DeleteIcon = string.Empty
            };

            var unrated3 = new Activity()
            {
                Name = "OTHER",
                IsEnabled = true,
                Rated = false,
                StudyId = Utilities.StudyId,
                DeleteIcon = string.Empty
            };

            ActivityRepo.SaveItem(unrated1);
            ActivityRepo.SaveItem(unrated2);
            ActivityRepo.SaveItem(unrated3);
        }
    }
}
