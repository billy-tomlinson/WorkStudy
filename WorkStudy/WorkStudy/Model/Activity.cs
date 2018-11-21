using System;
using System.Collections.Generic;
using System.Drawing;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WorkStudy.Model
{
    [Table("Activity")] 
    public class Activity : BaseEntity
    {
        public Activity()
        {
            Date = DateTime.Now;
            Colour = Color.Aquamarine;
            Activities  = new List<Activity>();
        }

        public string Name { get; set; }

        public string Comment { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime Date { get; set; }
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
