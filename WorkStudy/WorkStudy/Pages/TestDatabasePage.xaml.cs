using System;
using System.Linq;
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
        private readonly IBaseRepository<Observation> observationRepo;
        public TestDatabasePage()
        {
            InitializeComponent();
            sampleRepo = new BaseRepository<ActivitySampleStudy>(App.DatabasePath);
            activityRepo = new BaseRepository<Activity>(App.DatabasePath);
            operatorRepo = new BaseRepository<Operator>(App.DatabasePath);
            observationRepo = new BaseRepository<Observation>(App.DatabasePath);
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

        void Submit6_Clicked(object sender, System.EventArgs e)
        {
            AddAndRetrieveObservation();
        }

        void Submit7_Clicked(object sender, System.EventArgs e)
        {
            GetObservations();
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
            var list = sampleRepo.GetItems();
            TotalStudies.Text = list.ToList().Count.ToString();
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
            var list = activityRepo.GetItems();
            TotalActivities.Text = list.ToList().Count.ToString();
        }

        public void AddAndRetrieveOperator()
        {
            var operator1 = new Operator()
            {
                Name = "Activity One",
                StudyId = 1
                //LinkedActivitiesId = 1
            };

            var id = operatorRepo.SaveItem(operator1);

            var value = operatorRepo.GetItem(id);
            OperatorId.Text = value.Id.ToString();
        }

        public void GetOperators()
        {
            var list = operatorRepo.GetItems();
            TotalOperators.Text = list.ToList().Count.ToString();
        }

        public void AddAndRetrieveObservation()
        {
            var observation = new Observation()
            {
                ActivityId = 1,
                Date = DateTime.Now,
                OperatorId = 1,
                Rating = 65
            };

            var id = observationRepo.SaveItem(observation);

            var value = observationRepo.GetItem(id);
            ObservationId.Text = value.Id.ToString();
        }

        public void GetObservations()
        {
            var list = observationRepo.GetItems();
            TotalObservations.Text = list.ToList().Count.ToString();
        }
    }
}
