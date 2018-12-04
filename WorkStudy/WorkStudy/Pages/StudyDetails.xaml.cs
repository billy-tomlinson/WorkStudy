
using System;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy
{
    public partial class StudyDetails : ContentPage
    {
        public StudyDetails()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            studyName.Completed += studyName_Completed;
            studyDepartment.Completed += studyDepartment_Completed;
            studiedBy.Completed += studiedBy_Completed;
               
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(Utilities.StudyDetailsActive)
                studyName.Focus();
        }

        public void studyName_Completed(object sender, EventArgs e)
        {
            studyDepartment.Focus();
        }

        public void studyDepartment_Completed(object sender, EventArgs e)
        {
            studiedBy.Focus();
        }

        public void studiedBy_Completed(object sender, EventArgs e)
        {
            studyRated.Focus();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
