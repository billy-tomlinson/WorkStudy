using System.Collections.Generic;
using System.Linq;
using SQLite;
using SQLiteNetExtensions.Extensions;
using WorkStudy.Model;

namespace WorkStudy.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        string dbPath;
        string connectionString;

        public BaseRepository(string dbPath = null)
        {
            this.dbPath = dbPath;
            connectionString = dbPath == null ? App.DatabasePath : dbPath;
        }

        public IEnumerable<T> GetItems()
        {
            using(SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                return connection.Table<T>().ToList().OrderBy(x => x.Id); 
            }
        }


        public IEnumerable<T> GetAllWithChildren()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                return connection.GetAllWithChildren<T>().OrderBy(x => x.Id);
            }
        }


        public T GetWithChildren(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                return connection.GetWithChildren<T>(id);
            }
        }

        public int GetItemsCount()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                return connection.Table<T>().Count();
            }
        }

        public T GetItem(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                return connection.Table<T>().FirstOrDefault(i => i.Id == id);
            }
        }

        public int SaveItem(T item)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                if (item.Id != 0)
                {
                    connection.Update(item);
                    return 0;
                }
                connection.Insert(item);
                return GetId();
            }
        }


        public int DeleteItem(T item)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                return connection.Delete(item);
            }
        }

        public void UpdateWithChildren(T item)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.UpdateWithChildren(item);
            }
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        private int GetId()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                return connection.ExecuteScalar<int>("SELECT last_insert_rowid()");
            }
        }

        public void InsertOrReplaceWithChildren(T item)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.InsertOrReplaceWithChildren(item);
            }
        }

        public void CreateTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.CreateTable<T>();
            }
        }
    }
}
