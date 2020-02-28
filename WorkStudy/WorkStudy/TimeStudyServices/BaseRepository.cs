﻿using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using SQLiteNetExtensions.Extensions;
using TimeStudy.Model;

namespace TimeStudy.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        string dbPath;
        string connectionString;

        public BaseRepository(string dbPath = null)
        {
            this.dbPath = dbPath;
            connectionString = dbPath == null ? WorkStudy.App.DatabasePath : dbPath;

            if (connectionString == null || connectionString == string.Empty)
                connectionString = Utilities.Connection;
        }

        public BaseRepository()
        {
            if (connectionString == null || connectionString == string.Empty)
                connectionString = Utilities.Connection;
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
            SetLastUpdatedTime(item);
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                if (item.Id != 0)
                {
                    connection.Update(item);
                    return item.Id;
                }
                connection.Insert(item);
                return connection.ExecuteScalar<int>("SELECT last_insert_rowid()");
            }
        }


        public int DeleteItem(T item)
        {
            SetLastUpdatedTime(item);
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                return connection.Delete(item);
            }
        }

        public void UpdateWithChildren(T item)
        {
            SetLastUpdatedTime(item);
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.UpdateWithChildren(item);
            }
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void InsertOrReplaceWithChildren(T item)
        {
            SetLastUpdatedTime(item);
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

        public void DropTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.DropTable<T>();
            }
        }

        public void DeleteAllItems()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.DeleteAll<T>();
            }
        }


        public void ExecuteSQLCommand(string sqlCommand)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Execute(sqlCommand);
            }
        }

        public T1 ExecuteScalarSQLCommand<T1>(string sqlCommand)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                return connection.ExecuteScalar<T1>(sqlCommand);
            }
        }

        private void SetLastUpdatedTime(T item)
        {
            Type typeParameterType = typeof(T);
            var name = typeParameterType.Name;

            switch (name)
            {
                case "WorkElement":
                    Utilities.WorkElementTableUpdated = true;
                    Utilities.RatedTimeStudyPageHasUpdatedWorkElementChanges = false;
                    break;
                case "RatedTimeStudy":
                    Utilities.RatedTimeStudyTableUpdated = true;
                    Utilities.ForeignElementsPageHasUpdatedRatedTimeStudyChanges = false;
                    Utilities.StandardElementsPageHasUpdatedRatedTimeStudyChanges = false;
                    break;
                default:
                    break;
            }
        }

        public void InsertAll(List<T> items)
        {
            SetLastUpdatedTime(items[0]);
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.InsertOrReplaceAllWithChildren(items);
            }
        }

    }
}
