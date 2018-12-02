﻿using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;
using WorkStudy.Services;

namespace WorkStudy.Model
{
    [Table("Operator")]    
    public class Operator : BaseEntity
    {
        public Operator()
        {
            Activities = new List<Activity>();
            StudyId = Utilities.StudyId;
        }

        [ForeignKey(typeof(ActivitySampleStudy))]
        public int StudyId { get; set; }

        public string Name { get; set; }

        public DateTime Date => DateTime.Now;

        public bool IsEnabled { get; set; }

        public string Observed { get; set; }

        public bool Isvisible { get; set; }

        [ManyToMany(typeof(OperatorActivity))]
        public List<Activity> Activities { get; set; }

        [OneToMany]
        public List<Observation> Observations { get; set; }
    }
}
