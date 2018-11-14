using SQLite;

namespace WorkStudy.Model
{
    [Table("MergedActivities")] 
    public class MergedActivities : BaseEntity
    {
        public int ActivityId { get; set; }
        public int MergedActivityId { get; set; }
    }
}
