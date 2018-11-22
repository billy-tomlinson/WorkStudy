using System;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class StudyDetailsViewModel : BaseViewModel
    {
        public Command CloseView { get; set; }

        public StudyDetailsViewModel(string conn) : base(conn) { ConstructorSetUp(); }

        public StudyDetailsViewModel(){ ConstructorSetUp(); }

        override public void SubmitDetailsAndNavigate()
        {
            ValidateValues();

            if(!IsInvalid)
            {
                SampleRepo.SaveItem(SampleStudy);

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


        bool isInvalid;
        public bool IsInvalid
        {
            get { return isInvalid; }
            set
            {
                isInvalid = value;
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
            IsInvalid = true;

            if ((SampleStudy.Department != null &&  SampleStudy.Department?.Trim().Length > 0) &&
                (SampleStudy.Name != null && SampleStudy.Name?.Trim().Length > 0) &&
                (SampleStudy.StudiedBy != null && SampleStudy.StudiedBy?.Trim().Length > 0))

                IsInvalid = false;
        }

        private void CloseValidationView()
        {
            IsInvalid = false;
        }
    }
}
