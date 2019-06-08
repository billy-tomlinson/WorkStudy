﻿using System;
using SQLite;
using WorkStudy.Custom;
using WorkStudy.Services;

namespace WorkStudy.Model
{
    [Table("ActivitySampleStudy")]
    public class ActivitySampleStudy : BaseEntity
    {
        public string Name { get; set; }

        public string Department { get; set; }

        public string StudiedBy { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public int StudyNumber { get; set; }

        public bool IsRated { get; set; }

        public bool Completed { get; set; }

        [Ignore]
        public string ObservedColour { get; set; } = "#E8EAEC";

        [Ignore]
        public string DateTimeFormatted
        {
            get { return $"{Date.ToString("dd/MM/yyyy")} : {Time.ToString(@"hh\:mm")}"; }
        }

        [Ignore]
        public string DateFormatted
        {
            get { return $"{Date.ToString("dd/MM/yyyy")}"; }
        }

        [Ignore]
        public string TimeFormatted
        {
            get { return $"{Time.ToString(@"hh\:mm")}"; }
        }

    }
}