﻿using System;
using SQLiteNetExtensions.Attributes;

namespace WorkStudy.Model
{
    public class Observation : BaseEntity
    {
        [ForeignKey(typeof(Operator))]
        public int OperatorId { get; set; }

        [ForeignKey(typeof(Activity))]
        public int ActivityId { get; set; }

        public DateTime Date { get; set; }

        public int Rating { get; set; }
    }
}
