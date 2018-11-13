using System;
using System.Collections.Generic;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class TestDatabasePage : ContentPage
    {
        DataAccessService service;
        public TestDatabasePage()
        {
            InitializeComponent();
            service = new DataAccessService();
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


            var id = service.AddActivitySampleStudy(activityStudy);

            var sample = service.GetActivitySampleStudyById(id);
            StudyId.Text = sample.Id.ToString();
        }

        public void GetActivitySamples()
        {
            var studies = service.GetAllActivitySampleStudies();
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

            var id = service.AddActivity(activity);

            var value = service.GetActivityById(id);
            ActivityId.Text = value.Id.ToString();
        }

        public void GetActivities()
        {
            var studies = service.GetAllActivities();
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

            var id = service.AddOperator(operator1);

            var value = service.GetOperatorById(id);
            OperatorId.Text = value.Id.ToString();
        }

        public void GetOperators()
        {
            var operatorsList = service.GetAllOperators();
            TotalOperators.Text = operatorsList.Count.ToString();
        }
    }
}
