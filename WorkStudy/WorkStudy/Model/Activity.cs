using System;
using System.Collections.Generic;
using System.Drawing;
using SQLite;
using SQLiteNetExtensions.Attributes;
using WorkStudy.Services;

namespace WorkStudy.Model
{
    [Table("Activity")] 
    public class Activity : BaseEntity
    {
        public Activity()
        {
            Colour = Utilities.UnClicked;
            Activities  = new List<Activity>();
            StudyId = Utilities.StudyId;
        }

        public string Name { get; set; }

        [ForeignKey(typeof(ActivitySampleStudy))]
        public int StudyId { get; set; }

        public string Comment { get; set; }

        public bool IsEnabled { get; set; }

        public bool Rated { get; set; }

        public DateTime Date => DateTime.Now;

        public double Opacity { get; set; } = 1;

        [Ignore]
        public Color Colour { get; set; }

        [ManyToMany(typeof(OperatorActivity))]
        public List<Operator> Operators { get; set; }

        [ManyToMany(typeof(MergedActivities), "ActivityId", "MergedActivities",
        CascadeOperations = CascadeOperation.All)]
        public List<Activity> Activities { get; set; }

        [ManyToMany(typeof(MergedActivities), "MergedActivityId", "Activities",
        CascadeOperations = CascadeOperation.All, ReadOnly = true)]
        public List<Activity> MergedActivities { get; set; }

    }
}
