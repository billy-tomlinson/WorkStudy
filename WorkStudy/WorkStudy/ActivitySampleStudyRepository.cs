using System.Collections.Generic;

namespace WorkStudy
{
    public class ActivitySampleStudyRepository : IActivitySampleStudyRepository  
    {  
        DatabaseHelper _databaseHelper;  
        public ActivitySampleStudyRepository(){  
            _databaseHelper = new DatabaseHelper();  
        }  

        public List<ActivitySampleStudy> GetAllActivitySampleStudies()
        {
            return _databaseHelper.GetAllStudies(); 
        }

        public ActivitySampleStudy GetActivitySampleStudy(int id)
        {
            return _databaseHelper.GetStudy(id); 
        }

        public void DeleteAllStudies()
        {
            _databaseHelper.DeleteAllStudies();
        }

        public void DeleteStudy(int id)
        {
            _databaseHelper.DeleteStudy(id);
        }

        public void InsertStudy(ActivitySampleStudy study)
        {
            _databaseHelper.InsertStudy(study);
        }

        public void UpdateStudy(ActivitySampleStudy study)
        {
            _databaseHelper.UpdateStudy(study);
        }
    }  
}
