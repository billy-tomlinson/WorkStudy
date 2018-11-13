using System;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class TestDatabasePage : ContentPage
    {
        private readonly IBaseRepository<ActivitySampleStudy> sampleRepo;
        private readonly IBaseRepository<Activity> activityRepo;
        private readonly IBaseRepository<Operator> operatorRepo;
        public TestDatabasePage()
        {
            InitializeComponent();
            sampleRepo = new BaseRepository<ActivitySampleStudy>(App.DatabasePath);
            activityRepo = new BaseRepository<Activity>(App.DatabasePath);
            operatorRepo = new BaseRepository<Operator>(App.DatabasePath);
        }

        void Submit_Clicked(object sender, System.EventArgs e)
        {
            AddAndRetrieveActivitySample();
        }

        void Submit1_Clicked(object sender, System.EventArgs e)
        {
            GetActivitySamples();
        }


        void Submit2_Clicked(object sender, System.EventArgs e)
        {
            AddAndRetrieveActivity();
        }


        void Submit3_Clicked(object sender, System.EventArgs e)
        {
            GetActivities();
        }

        void Submit4_Clicked(object sender, System.EventArgs e)
        {
            AddAndRetrieveOperator();
        }

        void Submit5_Clicked(object sender, System.EventArgs e)
        {
            GetOperators();
        }

        public void AddAndRetrieveActivitySample()
        {
            var activityStudy = new ActivitySampleStudy()
            {
                Name = "Activity One",
                Date = DateTime.Now,
                StudyNumber = 1,
                IsRated = true,
                StudiedBy = "Billy",
                Department = "PaintShop"
            };


            var id = sampleRepo.SaveItem(activityStudy);

            var sample = sampleRepo.GetItem(id);
            StudyId.Text = sample.Id.ToString();
        }

        public void GetActivitySamples()
        {
            var studies = sampleRepo.GetItems();
            TotalStudies.Text = studies.Count.ToString();
        }

        public void AddAndRetrieveActivity()
        {
            var activity = new Activity()
            {
                Name = "Activity One",
                Comment = "Some comment or other",
                Date = DateTime.Now,
                IsEnabled = true
            };

            var id = activityRepo.SaveItem(activity);

            var value = activityRepo.GetItem(id);
            ActivityId.Text = value.Id.ToString();
        }

        public void GetActivities()
        {
            var studies = activityRepo.GetItems();
            TotalActivities.Text = studies.Count.ToString();
        }

        public void AddAndRetrieveOperator()
        {
            var operator1 = new Operator()
            {
                Name = "Activity One",
                StudyId = 1,
                LinkedActivitiesId = 1
            };

            var id = operatorRepo.SaveItem(operator1);

            var value = operatorRepo.GetItem(id);
            OperatorId.Text = value.Id.ToString();
        }

        public void GetOperators()
        {
            var operatorsList = operatorRepo.GetItems();
            TotalOperators.Text = operatorsList.Count.ToString();
        }
    }
}
