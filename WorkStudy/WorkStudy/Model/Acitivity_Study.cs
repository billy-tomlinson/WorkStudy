using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WorkStudy.Model
{
    [Table("Acitivity_Study")]
    public class Acitivity_Study : BaseEntity
    {
        [ForeignKey(typeof(ActivitySampleStudy))]
        public int StudyId { get; set; }

        [ForeignKey(typeof(Activity))]
        public int ActivityId { get; set; }
    }
}
