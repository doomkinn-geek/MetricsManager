using Dapper;
using MetricsAgent.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DAL.Repositories
{
    public static class AbstractRepository
    {
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";

        public static void AbstractCreate(MetricContainer item, string tableName)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                //  запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO " +
                    tableName +
                    " (value, time) VALUES(@value, @time)",
                    // анонимный объект с параметрами запроса
                    new
                    {
                        // value подставится на место "@value" в строке запроса
                        // значение запишется из поля Value объекта item
                        value = item.Value,

                        // записываем в поле time количество секунд
                        time = item.Time.TotalSeconds
                    });
            }
        }

        public static void AbstractDelete(int id, string tableName)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM " +
                    tableName +
                    " WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public static void AbstractUpdate(MetricContainer item, string tableName)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE " +
                    tableName + " SET value = @value, time = @time WHERE id=@id",
                    new
                    {
                        value = item.Value,
                        time = item.Time.TotalSeconds,
                        id = item.Id
                    });
            }
        }

        public static IList<MetricContainer> AbstractGetAll(string tableName)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                // читаем при помощи Query и в шаблон подставляем тип данных
                // объект которого Dapper сам и заполнит его поля
                // в соответсвии с названиями колонок
                return connection.Query<MetricContainer>("SELECT Id, Time, Value FROM " + tableName).ToList();
            }
        }

        public static MetricContainer AbstractGetById(int id, string tableName)
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    return connection.QuerySingle<MetricContainer>("SELECT Id, Time, Value FROM " +
                        tableName + " WHERE id=@id",
                        new { id = id });
                }
            }
            catch(Exception e)
            {
                return new MetricContainer { Id = 0 };
            }
        }

        public static IList<MetricContainer> AbstractGetByTimePeriod(TimeSpan fromTime, TimeSpan toTime, string tableName)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                // читаем при помощи Query и в шаблон подставляем тип данных
                // объект которого Dapper сам и заполнит его поля
                // в соответсвии с названиями колонок
                return connection.Query<MetricContainer>("SELECT Id, Time, Value FROM " +
                    tableName +
                    " WHERE Time >= @fromTime AND Time <= toTime").ToList();
            }
        }
    }
}
