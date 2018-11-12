using System.Collections.Generic;
using SQLite;

namespace WorkStudy.Model
{
    [Table("Activity")] 
    public class Activity
    {
        public int Id{ get; set;}
        public string Name { get; set; }
        public string Comment { get; set; }
        [Ignore]
        public List<Activity> GroupedActivities { get; set; }
    }
}
