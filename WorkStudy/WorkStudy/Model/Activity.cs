using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.Model
{
    [Table("Activity")] 
    public class Activity : BaseEntity
    {
        public Activity()
        {
            Colour = Utilities.UnClicked;
            Activities  = new List<Activity>();
            //StudyId = Utilities.StudyId;
        }

        public string Name { get; set; }

        //[ForeignKey(typeof(ActivitySampleStudy))]
        //public int StudyId { get; set; }
        //[Ignore]
        //public int StudyId { get; set; }

        public string Comment { get; set; }

        public bool IsEnabled { get; set; }

        public bool Rated { get; set; }

        public DateTime Date => DateTime.Now;

        public double Opacity { get; set; } = 1;

        [Ignore]
        public Color Colour { get; set; }

        [Ignore]
        public bool Selected { get; set; }

        [ManyToMany(typeof(MergedActivities), "ActivityId", "MergedActivities",
        CascadeOperations = CascadeOperation.All)]
        public List<Activity> Activities { get; set; }

        [ManyToMany(typeof(MergedActivities), "MergedActivityId", "Activities",
        CascadeOperations = CascadeOperation.All, ReadOnly = true)]
        public List<Activity> MergedActivities { get; set; }

        [ManyToMany(typeof(Acitivity_Study), "ActivityId", "Acitivity_Study",
        CascadeOperations = CascadeOperation.All)]
        public List<ActivitySampleStudy> ActivitySampleStudies { get; set; }

        public string ObservedColour { get; set; } = "#d5f0f1";

        [Ignore]
        public Color ConvertedColour => Color.FromHex(ObservedColour);

        public string DeleteIcon { get; set; } = "delete.png";

        public string SettingsIcon { get; set; } = "comments.png";

    }
}
