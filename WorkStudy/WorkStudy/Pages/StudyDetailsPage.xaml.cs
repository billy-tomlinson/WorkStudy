
using System;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class StudyDetailsPage : ContentPage
    {
        public StudyDetailsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            studyName.Completed += studyName_Completed;
            studyDepartment.Completed += studyDepartment_Completed;
            studiedBy.Completed += studiedBy_Completed;
               
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
            studiedBy.Focus();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Utilities.ClearNavigation();
        }
    }
}
