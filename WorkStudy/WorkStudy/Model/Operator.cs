using System;
using SQLite;

namespace WorkStudy.Model
{
    [Table("Operator")]    
    public class Operator : BaseEntity
    {
        public int StudyId { get; set; }
        public string Name { get; set; }
        public int LinkedActivitiesId { get; set; }
        public DateTime Date { get; set; }
        public bool IsEnabled { get; set; }
    }
}
