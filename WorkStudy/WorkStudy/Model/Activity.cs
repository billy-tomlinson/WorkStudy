using System;
using SQLite;

namespace WorkStudy.Model
{
    [Table("Activity")] 
    public class Activity : BaseEntity
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public bool IsEnabled { get; set; }
        public int? MergedActivities { get; set; }
        public DateTime Date { get; set; }
    }
}
