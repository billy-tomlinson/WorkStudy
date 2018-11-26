using System;
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
                SampleRepo.SaveItem(SampleStudy);

                CreateUnratedActivities();

                Utilities.RatedStudy = SampleStudy.IsRated;

                Utilities.Navigate(new AddActivities());
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
            SampleStudy = new ActivitySampleStudy() 
            { 
                IsRated = true, 
                Date = DateTime.Now,
                Time = DateTime.Now.TimeOfDay
            };

            Utilities.StudyId = SampleRepo.SaveItem(SampleStudy);
            SampleStudy.StudyNumber = Utilities.StudyId;
            CloseView = new Command(CloseValidationView);

        }

        private void ValidateValues()
        {
            ValidationText = "Please enter all study details";

            IsInvalid = true;

            if ((SampleStudy.Department != null &&  SampleStudy.Department?.Trim().Length > 0) &&
                (SampleStudy.Name != null && SampleStudy.Name?.Trim().Length > 0) &&
                (SampleStudy.StudiedBy != null && SampleStudy.StudiedBy?.Trim().Length > 0))

                IsInvalid = false;
        }

        public void CreateUnratedActivities()
        {
            var unrated1 = new Activity()
            {
                Name = "Idle",
                IsEnabled = true,
                Rated = false,
                StudyId = Utilities.StudyId
            };

            var unrated2 = new Activity()
            {
                Name = "Inactive",
                IsEnabled = true,
                Rated = false,
                StudyId = Utilities.StudyId
            };

            var unrated3 = new Activity()
            {
                Name = "Other",
                IsEnabled = true,
                Rated = false,
                StudyId = Utilities.StudyId
            };

            ActivityRepo.SaveItem(unrated1);
            ActivityRepo.SaveItem(unrated2);
            ActivityRepo.SaveItem(unrated3);
        }
    }
}
