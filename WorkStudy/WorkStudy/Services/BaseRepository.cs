using System.Collections.Generic;
using SQLite;
using WorkStudy.Model;

namespace WorkStudy.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        private static readonly object locker = new object();
        private readonly SQLiteConnection databaseConnection;

        public BaseRepository()
        {
            databaseConnection = new SQLiteConnection(App.DatabasePath);
            databaseConnection.CreateTable<T>();
        }

        public BaseRepository(string dbPath)
        {
            databaseConnection = new SQLiteConnection(dbPath);
            databaseConnection.CreateTable<T>();
        }

        public List<T> GetItems()
        {
            lock (locker)
            {
                return databaseConnection.Table<T>().ToList();
            }
        }

        public int GetItemsCount()
        {
            lock (locker)
            {
                return databaseConnection.Table<T>().Count();
            }
        }

        public T GetItem(int id)
        {
            lock (locker)
            {
                return databaseConnection.Table<T>().FirstOrDefault(i => i.Id == id);
            }
        }

        public int SaveItem(T item)
        {
            lock (locker)
            {
                if (item.Id != 0)
                    return databaseConnection.Update(item);

                databaseConnection.Insert(item);
                return GetId();
            }
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        private int GetId()
        {
            return databaseConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
        }
    }
}
