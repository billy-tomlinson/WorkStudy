﻿using System;
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
            SetLastUpdatedTime(item);
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                if (item.Id != 0)
                {
                    connection.Update(item);
                    return 0;
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

        private void SetLastUpdatedTime(T item)
        {
            Type typeParameterType = typeof(T);
            var name = typeParameterType.Name;

            switch (name)
            {
                case "Operator":
                    Utilities.OperatorTableUpdated = true;
                    Utilities.ActivityPageHasUpdatedOperatorChanges = false;
                    Utilities.MainPageHasUpdatedOperatorChanges = false;
                    break;
                case "Activity":
                    Utilities.ActivityTableUpdated = true;
                    Utilities.ActivityPageHasUpdatedActivityChanges = false;
                    Utilities.MainPageHasUpdatedActivityChanges = false;
                    Utilities.MergePageHasUpdatedActivityChanges = false;
                    Utilities.AllActivitiesPageHasUpdatedActivityChanges = false;
                    break;
                case "Observation":
                    Utilities.ObservationTableUpdated = true;
                    Utilities.ActivityPageHasUpdatedObservationChanges = false;
                    Utilities.MainPageHasUpdatedObservationChanges = false;
                    break;
                default:
                    break;
            }
        }
    }
}
