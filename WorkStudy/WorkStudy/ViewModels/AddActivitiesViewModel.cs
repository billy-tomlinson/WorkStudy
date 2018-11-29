﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class AddActivitiesViewModel : BaseViewModel
    {
        public Command SaveActivity { get; set; }
        public Command SaveComment { get; set; }
        public Command CancelComment { get; set; }
        public Command ItemSelected { get; set; }
        public Command SettingsSelected { get; set; }
        public Command DeleteSelected { get; set; }
        public Activity Activity;

        public AddActivitiesViewModel()
        {           
            ConstructorSetUp();
        }

        public AddActivitiesViewModel(string conn) : base(conn)
        {
            ConstructorSetUp();
        }

        static ObservableCollection<Activity> itemsCollection;
        public ObservableCollection<Activity> ItemsCollection
        {
            get => itemsCollection;
            set
            {
                itemsCollection = value;
                OnPropertyChanged();
            }
        }

        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                comment = value;
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

        static bool commentsVisible;
        public bool CommentsVisible
        {
            get => commentsVisible;
            set
            {
                commentsVisible = value;
                OnPropertyChanged();
            }
        }

        void SaveActivityDetails()
        {
            ValidateValues();

            if (!IsInvalid)
            {
                var duplicatesCheck = new List<Activity>(ItemsCollection);
                if (duplicatesCheck.Find(_ => _.Name.ToUpper() == Name.ToUpper().Trim()) == null)
                    ActivityRepo.SaveItem(new Activity { Name = Name.ToUpper().Trim(), IsEnabled = true, Rated = true});
                ItemsCollection = Get_Rated_Enabled_Activities();

                Name = string.Empty;
            }           
        }

        void SaveCommentDetails()
        {
            if (Comment != null)
            {
                Activity.Comment = Comment.ToUpper();
                ActivityRepo.SaveItem(Activity);
            }
           
            CommentsVisible = false;
            Comment = string.Empty;
        }

        void CancelCommentDetails()
        {
            CommentsVisible = false;
            Comment = string.Empty;
        }

        public override void SubmitDetailsAndNavigate()
        {
            ValidateActivitiesAdded();

            if (!IsInvalid)
            {
                Utilities.Navigate(new AddOperators());
            }
        }

        public ICommand ItemClickedCommand
        {
            get { return ShowComments(); }
        }

        Command ShowComments()
        {
            return new Command((item) =>
            {
                Activity = item as Activity;
                Comment = Activity.Comment;
                CommentsVisible = true;
            });
        }

        public void ValidateValues()
        {
            ValidationText = "Please Enter a valid Name";

            IsInvalid = true;

            if ((Name != null && Name?.Trim().Length > 0))
                IsInvalid = false;
        }

        private void ValidateActivitiesAdded()
        {
            
            ValidationText = "Please add at least one Activity";

            IsInvalid = true;

            var activities = Get_Rated_Enabled_Activities();

            if ((activities.Count > 0))
                IsInvalid = false;
        }

        void AddSelectedEvent(object sender)
        {
            var value = (int)sender;
            Activity = ActivityRepo.GetItem(value);
            Comment = Activity.Comment;
            CommentsVisible = true;
        }

        void DeleteSelectedEvent(object sender)
        {
            var value = (int)sender;
        }

        void ActivitySelectedEvent(object sender)
        {
            var value = (int)sender;
        }

        private void ConstructorSetUp()
        {
            SaveActivity = new Command(SaveActivityDetails);
            SaveComment = new Command(SaveCommentDetails);
            CancelComment = new Command(CancelCommentDetails);
            ItemSelected = new Command(ActivitySelectedEvent);
            SettingsSelected = new Command(AddSelectedEvent);
            DeleteSelected = new Command(DeleteSelectedEvent);

            Name = string.Empty;
            ItemsCollection = Get_Rated_Enabled_Activities();
            Activity = new Activity();
        }
    }
}

