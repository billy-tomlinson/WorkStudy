using System;
using System.Collections.Generic;
using SQLite;

namespace WorkStudy.Model
{
    [Table("Activity")] 
    public class Activity
    {
        [PrimaryKey, AutoIncrement]  
        public int Id{ get; set;}
        public string Name { get; set; }
        public string Comment { get; set; }
        public bool IsEnabled { get; set; }
        public int? MergedActivities { get; set; }
        public DateTime Date { get; internal set; }
    }
}
