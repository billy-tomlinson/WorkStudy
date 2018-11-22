using System;
using WorkStudy.Model;
using WorkStudy.Services;

namespace WorkStudy.ViewModels
{
    public class StudyDetailsViewModel : BaseViewModel
    {
        public StudyDetailsViewModel(string conn) : base(conn) { ConstructorSetUp(); }

        public StudyDetailsViewModel(){ ConstructorSetUp(); }

        override public void SubmitDetailsAndNavigate()
        {
         
            SampleRepo.SaveItem(SampleStudy);

            Utilities.Navigate(new AddActivities());
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

        }
    }
}
