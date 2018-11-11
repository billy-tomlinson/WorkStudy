using SQLite;  
using Xamarin.Forms;  
using System.Collections.Generic;  
using System.Linq;     
  
namespace WorkStudy {  
    public class DatabaseHelper {  
          
        static SQLiteConnection sqliteconnection;  
        public const string DbFileName = "WorkStudy.db3";  
  
        public DatabaseHelper() {  
            sqliteconnection = DependencyService.Get<ISQLite>().GetConnection(); 
            sqliteconnection.CreateTable<ActivitySampleStudy>();

        }  
      
        public List<ActivitySampleStudy> GetAllStudies() {  
            return (from data in sqliteconnection.Table<ActivitySampleStudy>()  
                    select data).ToList();  
        }  
  
        public ActivitySampleStudy GetStudy(int id) {  
            return sqliteconnection.Table<ActivitySampleStudy>().FirstOrDefault(t => t.Id == id);  
        }  
  
        public void DeleteAllStudies() {  
            sqliteconnection.DeleteAll<ActivitySampleStudy>();  
        }  
   
        public void DeleteStudy(int id) {  
            sqliteconnection.Delete<ActivitySampleStudy>(id);  
        }  
   
        public void InsertStudy(ActivitySampleStudy study) {  
            sqliteconnection.Insert(study);  
        }  
  
        public void UpdateStudy(ActivitySampleStudy study) {  
            sqliteconnection.Update(study);  
        }  
    }  
}  
