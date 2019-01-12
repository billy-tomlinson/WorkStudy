using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

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

        [ManyToMany(typeof(Acitivity_Study), "StudyId", "Acitivity_Study",
        CascadeOperations = CascadeOperation.All)]
        public List<Activity> Activities { get; set; }

        [Ignore]
        public string DateTimeFormatted 
        { 
            get { return $"{Date.ToString("dd/MM/yyyy")} : {Time.ToString((@"hh\:mm"))}"; } 
        }

    }
}
