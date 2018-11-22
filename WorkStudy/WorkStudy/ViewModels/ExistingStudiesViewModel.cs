﻿using System.Collections.ObjectModel;
using System.Linq;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class ExistingStudiesViewModel : BaseViewModel
    {
        public ExistingStudiesViewModel()
        {
            ActivitySamples = new ObservableCollection<ActivitySampleStudy>(SampleRepo.GetItems()
                                                                            .Where(_ => _.Completed == false));
        }

        static ObservableCollection<ActivitySampleStudy> activitySamples;
        public ObservableCollection<ActivitySampleStudy> ActivitySamples
        {
            get => activitySamples;
            set
            {
                activitySamples = value;
                OnPropertyChanged();
            }
        }

        public Command ItemClickedCommand
        {
            get { return Navigate(); }
        }

        public Command Navigate()
        {
            return new Command((item) =>
            {
                var study = item as ActivitySampleStudy;
                Utilities.StudyId = study.Id;
                Utilities.Navigate(new MainPage());
            });
        }
    }
}
