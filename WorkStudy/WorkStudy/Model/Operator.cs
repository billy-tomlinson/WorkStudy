using System.Collections.Generic;
using SQLite;

namespace WorkStudy.Model
{
    [Table("Operator")]    
    public class Operator
    {
        [PrimaryKey, AutoIncrement]  
        public int Id { get; set; }
        public int StudyId { get; set; }
        public string Name { get; set; }
        public int LinkedActivitiesId { get; set; }
    }
}
