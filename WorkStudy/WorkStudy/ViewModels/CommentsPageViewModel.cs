﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class CommentsPageViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public Command SaveComment { get; set; }

        public CommentsPageViewModel()
        {
            SaveComment = new Command(SaveCommentDetails);
            Comment = string.Empty;
        }

        ObservableCollection<string> comments;
        public ObservableCollection<string> Comments
        {
            get { return Utilities.Comments; }
            set
            {
                if (comments == null)
                    comments = new ObservableCollection<string>();
                comments.Add("Billy");
                OnPropertyChanged();
            }
        }


        static string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void SaveCommentDetails()
        {
            if (comments == null)
            {
                comments = new ObservableCollection<string>();
                comments = Utilities.Comments;
            }
                
            comments.Add(Comment);
            Utilities.Comments = comments;
            Comment = string.Empty;

        }

    }
}


