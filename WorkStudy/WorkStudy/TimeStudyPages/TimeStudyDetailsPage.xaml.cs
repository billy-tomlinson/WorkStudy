﻿
using System;
using TimeStudy.Services;
using TimeStudy.ViewModels;
using Xamarin.Forms;

namespace TimeStudy.Pages
{
    public partial class TimeStudyDetailsPage : ContentPage
    {
        public TimeStudyDetailsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetBackButtonTitle(this, "");
            this.BackgroundColor = Color.FromHex(Utilities.TimeStudyBackGroundColour);

            studyName.Completed += studyName_Completed;
            studyDepartment.Completed += studyDepartment_Completed;
        }

        public void studyName_Completed(object sender, EventArgs e)
        {
            studyDepartment.Focus();
        }

        public void studyDepartment_Completed(object sender, EventArgs e)
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
