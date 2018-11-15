using System;
using System.Collections;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WorkStudy.Model
{
    [Table("Operator")]    
    public class Operator : BaseEntity
    {
        [ForeignKey(typeof(ActivitySampleStudy))]
        public int StudyId { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public bool IsEnabled { get; set; }

        public string Observed { get; set; }

        public bool Isvisible { get; set; }

        [ManyToMany(typeof(OperatorActivity))]
        public List<Activity> Activities { get; set; }
    }
}
