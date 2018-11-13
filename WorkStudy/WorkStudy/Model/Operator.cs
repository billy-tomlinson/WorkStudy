using System.Collections.Generic;
using SQLite;

namespace WorkStudy.Model
{
    [Table("Operator")]    
    public class Operator : BaseEntity
    {
        public int StudyId { get; set; }
        public string Name { get; set; }
        public int LinkedActivitiesId { get; set; }
    }
}
