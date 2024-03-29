﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class AddOperatorsViewModel : BaseViewModel
    {
        public Command SaveOperator { get; set; }
        public Command SaveActivities { get; set; }
        public Command CancelActivities { get; set; }
        public Command ActivitySelected { get; set; }
        public Command ItemSelected { get; set; }
        public Command SettingsSelected { get; set; }
        public Command DeleteSelected { get; set; }
        public Command CloseRunningTotals { get; set; }

        public AddOperatorsViewModel()
        {
            ConstructorSetUp();
        }
        public AddOperatorsViewModel(string conn) : base(conn)
        {
            ConstructorSetUp();
        }

        private ObservableCollection<Operator> itemsCollection;
        public ObservableCollection<Operator> ItemsCollection
        {
            get => itemsCollection;
            set
            {
                itemsCollection = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<OperatorRunningTotal> runningTotals;
        public ObservableCollection<OperatorRunningTotal> RunningTotals
        {
            get => runningTotals;
            set
            {
                runningTotals = value;
                OnPropertyChanged();
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        static bool activitiesVisible;
        public bool ActivitiesVisible
        {
            get => activitiesVisible;
            set
            {
                activitiesVisible = value;
                OnPropertyChanged();
            }
        }

        void SaveOperatorDetails()
        {
            ValidateValues();

            if (!IsInvalid)
            {
                Name = Name.ToUpper().Trim();

                List<Operator> duplicatesCheck = new List<Operator>(ItemsCollection);

                if (duplicatesCheck.Find(_ => _.Name.ToUpper() == Name.ToUpper().Trim()) == null)
                    Operator.Id = OperatorRepo.SaveItem(new Operator() 
                    { 
                        Name = Name, 
                        IsEnabled = true
                    });

                ItemsCollection = GetAllOperators();

                HasElements = ItemsCollection.Count > 0;

                Name = string.Empty;
            }
        }

        void DeleteSelectedEvent(object sender)
        {
            var value = (int)sender;

            Operator = OperatorRepo.GetItem(value);

            if(!StudyInProcess)
                OperatorRepo.DeleteItem(Operator); 
            else
            {
                if(Operator.DeleteIcon == Utilities.UndoImage)
                {
                    Operator.Opacity = 1;
                    Operator.IsEnabled = true;
                    Operator.DeleteIcon = Utilities.DeleteImage;
                }
                else
                {
                    Operator.Opacity = 0.2;
                    Operator.IsEnabled = false;
                    Operator.DeleteIcon = Utilities.UndoImage;
                }

                OperatorRepo.SaveItem(Operator);
            }
           
            ItemsCollection = GetAllOperators();

            HasElements = ItemsCollection.Count > 0;

            Activities = Get_Rated_Enabled_Activities_WithChildren();
        }

        void OperatorSelectedEvent(object sender)
        {
            var value = (int)sender;
            Operator = OperatorRepo.GetWithChildren(value);
            RunningTotals = new ObservableCollection<OperatorRunningTotal>(GetRunningTotals(Operator));
            if(RunningTotals.Count == 0)
            {
                Opacity = 0.2;
                ValidationText = "More observations are required for accuracy.";
                ShowClose = true;
                IsInvalid = true;
                IsPageEnabled = false;
            }
            else 
            {
                Opacity = 0.2;
                RunningTotalsVisible = true;
                IsPageEnabled = false;
            }

        }

        void CloseRunningTotalsEvent(object sender)
        {
            Opacity = 1.0;
            RunningTotalsVisible = false;
            IsPageEnabled = true;
            Utilities.CloseRunningTotals = true;
        }

        private void ConstructorSetUp()
        {
            SaveOperator = new Command(SaveOperatorDetails);
            ItemSelected = new Command(OperatorSelectedEvent);
            DeleteSelected = new Command(DeleteSelectedEvent);
            CloseRunningTotals = new Command(CloseRunningTotalsEvent);

            ItemsCollection = GetAllOperators();

            HasElements = ItemsCollection.Count > 0;

            Activities = Get_Rated_Enabled_Activities_WithChildren();
            Operator = new Operator();
            Operator.SettingsIcon = string.Empty;
            Name = string.Empty;
            Opacity = 1.0;
            IsPageEnabled = true;
        }

        private void ValidateValues()
        {
            ValidationText = "Please Enter a valid Name";
            Opacity = 0.2;
            IsInvalid = true;
            IsPageEnabled = false;
            ShowClose = true;

            if ((Name != null && Name?.Trim().Length > 0))
            {
                if (Name == "1" || Name.ToUpper() == "I")
                    Name = Name + ".";

                Opacity = 1;
                IsInvalid = false;
                IsPageEnabled = true;
            }    
        }

        public void ValidateOperatorActivities()
        {
            ShowClose = true;
            IsInvalid = true;
            IsPageEnabled = false;
            Opacity = 0.2;

            var studyOperators = OperatorRepo.GetAllWithChildren()
                                          .Where(_ => _.StudyId == Utilities.StudyId).ToList();
            if (!studyOperators.Any())
            {
                ValidationText = "Add at least one operator.";
                return;
            }

            IsInvalid = false;
            IsPageEnabled = true;
            Opacity = 1;
        }

        private ObservableCollection<Operator> GetAllOperators()
        {
            return new ObservableCollection<Operator>(OperatorRepo.GetAllWithChildren()
                       .Where(_ => _.StudyId == Utilities.StudyId).OrderByDescending(x => x.Id));
        }
    }
}

