using System.Collections.Generic;
using SQLite;
using WorkStudy.Model;

namespace WorkStudy.Services
{
    public class DataAccessService
    {
        public SQLiteConnection connection { get; set;}

        public DataAccessService()
        {
            connection = new SQLiteConnection(App.DatabasePath);
        }

        public DataAccessService(string connectionString)
        {
            connection = new SQLiteConnection(connectionString);
        }

        public int  AddActivitySampleStudy(ActivitySampleStudy value)
        {

            connection.CreateTable<ActivitySampleStudy>();
            connection.Insert(value);
            return GetId();
        }

        public List<ActivitySampleStudy> GetAllActivitySampleStudies()
        {
            connection.CreateTable<ActivitySampleStudy>();
            List<ActivitySampleStudy> values = connection.Table<ActivitySampleStudy>().ToList();
            return values;
        }

        public ActivitySampleStudy GetActivitySampleStudyById(int id)
        {
            connection.CreateTable<ActivitySampleStudy>();
            var value = connection.Table<ActivitySampleStudy>().Where(_ => _.Id == id).FirstOrDefault();
            return value;
        }

        public int AddActivity(Activity value)
        {
            connection.CreateTable<Activity>();
            connection.Insert(value);
            return GetId();
        }

        public List<Activity> GetAllActivities()
        {
            connection.CreateTable<Activity>();
            List<Activity> values = connection.Table<Activity>().ToList();
            return values;
        }

        public Activity GetActivityById(int id)
        {
            connection.CreateTable<Activity>();
            var value = connection.Table<Activity>().Where(_ => _.Id == id).FirstOrDefault();
            return value;
        }

        public int AddOperator(Operator value)
        {
            connection.CreateTable<Operator>();
            var Id = connection.Insert(value);
            return GetId();
        }

        public List<Operator> GetAllOperators()
        {
            connection.CreateTable<Operator>();
            List<Operator> values = connection.Table<Operator>().ToList();
            return values;
        }

        public Operator GetOperatorById(int id)
        {
            connection.CreateTable<Operator>();
            var value = connection.Table<Operator>().Where(_ => _.Id == id).FirstOrDefault();
            return value;
        }

        private int GetId()
        {
            return connection.ExecuteScalar<int>("SELECT last_insert_rowid()");
        }

    }
}
