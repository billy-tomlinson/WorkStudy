using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WorkStudy.Model
{
    [Table("ActivityName")]
    public class ActivityName : BaseEntity
    {
        public string Name { get; set; }
    }
}
