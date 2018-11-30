﻿using System.Windows.Input;
using WorkStudy.Pages;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class MenuPageViewModel
    {
        public ICommand StudyMenu { get; set; }
        public ICommand AddActivities { get; set; }
        public ICommand MergeActivities { get; set; }
        public ICommand AddOperators { get; set; }
        public ICommand CompletedStudies { get; set; }
        public ICommand PausedStudies { get; set; }
        public ICommand Reports { get; set; }

        public MenuPageViewModel()
        {
            StudyMenu = new Command(GoStudyMenu);
            AddActivities = new Command(GoActivities);
            MergeActivities = new Command(GoMergeActivities);
            AddOperators = new Command(GoOperators);
            CompletedStudies = new Command(GoCompletedStudies);
            PausedStudies = new Command(GoPausedStudies);
            Reports = new Command(GoReports);
        }

        void GoStudyMenu(object obj)
        {
            Utilities.StudyId = 0;
            Utilities.Navigate(new StudyMenu());
            App.MenuIsPresented = false;
        }

        void GoActivities(object obj)
        {
            Utilities.Navigate(new AddActivities()); 
            App.MenuIsPresented = false;
        }

        void GoMergeActivities(object obj)
        {
            Utilities.Navigate(new EditActivities());
            App.MenuIsPresented = false;
        }

        void GoOperators(object obj)
        {
            Utilities.Navigate(new AddOperators());
            App.MenuIsPresented = false;
        }

        void GoCompletedStudies(object obj)
        {
            Utilities.Navigate(new CompletedStudiesPage(true));
            App.MenuIsPresented = false;
        }

        void GoPausedStudies(object obj)
        {
            Utilities.Navigate(new PausedStudiesPage(false));
            App.MenuIsPresented = false;
        }

        void GoReports(object obj)
        {
            Utilities.Navigate(new ReportsPage());
            App.MenuIsPresented = false;
        }

       
    }
}