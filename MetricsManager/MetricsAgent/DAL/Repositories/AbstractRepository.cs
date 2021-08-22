using Dapper;
using MetricsAgent.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DAL.Repositories
{
    public abstract class AbstractRepository
    {
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        public string TableName { get; set;}

        public AbstractRepository(string _tablename)
        {
            TableName = _tablename;
        }
        public void AbstractCreate(Metric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                //  запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO " +
                    TableName +
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

        public void AbstractDelete(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM " +
                    TableName +
                    " WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public void AbstractUpdate(Metric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE " + TableName + " SET value = @value, time = @time WHERE id=@id", new
                {
                    value = item.Value,
                    time = item.Time.TotalSeconds,
                    id = item.Id
                });
            }
        }

        public IList<Metric> AbstractGetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                // читаем при помощи Query и в шаблон подставляем тип данных
                // объект которого Dapper сам и заполнит его поля
                // в соответсвии с названиями колонок
                return connection.Query<Metric>("SELECT Id, Time, Value FROM " + TableName).ToList();
            }
        }

        public Metric AbstractGetById(int id)
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    return connection.QuerySingle<Metric>("SELECT Id, Time, Value FROM " +
                        TableName + " WHERE id=@id",
                        new { id = id });
                }
            }
            catch(Exception)
            {
                return new Metric { Id = 0 };
            }
        }

        public IList<Metric> AbstractGetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                // читаем при помощи Query и в шаблон подставляем тип данных
                // объект которого Dapper сам и заполнит его поля
                // в соответсвии с названиями колонок
                return connection.Query<Metric>("SELECT Id, Time, Value FROM " +
                    TableName +
                    " WHERE Time >= @from AND Time <= @to", new
                    {
                        from = fromTime.Ticks,
                        to = toTime.Ticks
                    }).ToList();
            }
        }
    }
}
