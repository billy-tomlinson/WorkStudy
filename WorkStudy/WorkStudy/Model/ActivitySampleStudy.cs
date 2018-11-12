using System;
using SQLite;

namespace WorkStudy.Model
{
    [Table("ActivitySampleStudy")] 
    public class ActivitySampleStudy
    {     
        [PrimaryKey, AutoIncrement]  
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string StudiedBy { get; set; }
        public DateTime Date { get; set; }
        public int StudyNumber { get; set; }
        public bool IsRated { get; set; }

    }
}
