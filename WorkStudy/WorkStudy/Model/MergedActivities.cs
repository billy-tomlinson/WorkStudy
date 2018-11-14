using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WorkStudy.Model
{
    [Table("MergedActivities")] 
    public class MergedActivities : BaseEntity
    {
        [ForeignKey(typeof(Activity))]
        public int ActivityId { get; set; }

        [ForeignKey(typeof(Activity))]
        public int MergedActivityId { get; set; }
    }
}
