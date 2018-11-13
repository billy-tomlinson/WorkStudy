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

        public int Add<T>(T value)
        {
            connection.CreateTable<T>();
            connection.Insert(value);
            return GetId();
        }

        public List<T> GetAll<T>() where T : new()
        {
            connection.CreateTable<T>();
            List<T> values = connection.Table<T>().ToList();
            return values;
        }

        public ActivitySampleStudy GetActivitySampleStudyById(int id)
        {
            connection.CreateTable<ActivitySampleStudy>();
            var value = connection.Table<ActivitySampleStudy>().Where(_ => _.Id == id).FirstOrDefault();
            return value;
        }

        public Activity GetActivityById(int id)
        {
            connection.CreateTable<Activity>();
            var value = connection.Table<Activity>().Where(_ => _.Id == id).FirstOrDefault();
            return value;
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
