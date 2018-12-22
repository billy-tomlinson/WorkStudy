using SQLiteNetExtensions.Attributes;

namespace WorkStudy.Model
{
    public class ObservationRoundStatus : BaseEntity
    {
        [ForeignKey(typeof(Observation))]
        public int ObservationId { get; set; }

        [ForeignKey(typeof(ActivitySampleStudy))]
        public int StudyId { get; set; }

        public string Status { get; set; }
    }
}
