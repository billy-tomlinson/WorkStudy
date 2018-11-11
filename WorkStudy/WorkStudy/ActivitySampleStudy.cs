using SQLite;

namespace WorkStudy
{
    [Table("ActivitySampleStudy")] 
    public class ActivitySampleStudy
    {     
        [PrimaryKey, AutoIncrement]  
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
