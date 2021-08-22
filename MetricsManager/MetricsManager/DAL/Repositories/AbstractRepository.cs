using Dapper;
using MetricsManager.DAL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Repositories
{
    public abstract class AbstractRepository
    {
        //private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        public string TableName { get; set;}
        protected IConfiguration Configuration { get; set; }
        private string ConnectionString { get; set; }

        public AbstractRepository(string _tablename, IConfiguration _configuration)
        {
            TableName = _tablename;
            this.Configuration = _configuration;
            ConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
            // добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }
        protected void AbstractCreate(Metric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                //  запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO " +
                    TableName +
                    " (agentId, value, time) VALUES(@agentId, @value, @time)",
                    // анонимный объект с параметрами запроса
                    new
                    {
                        agentId = item.AgentId,
                        value = item.Value,                        
                        time = item.Time.TotalSeconds
                    });
            }
        }

        protected void AbstractDelete(int id)
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

        protected void AbstractUpdate(Metric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE " + TableName + " SET agentId = @agentId, value = @value, time = @time WHERE id=@id", new
                {
                    value = item.Value,
                    time = item.Time.TotalSeconds,
                    id = item.Id
                });
            }
        }

        protected IList<Metric> AbstractGetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                // читаем при помощи Query и в шаблон подставляем тип данных
                // объект которого Dapper сам и заполнит его поля
                // в соответсвии с названиями колонок
                return connection.Query<Metric>("SELECT Id, agentId, Time, Value FROM " + TableName).ToList();
            }
        }

        protected Metric AbstractGetById(int id)
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    return connection.QuerySingle<Metric>("SELECT Id, agentId, Time, Value FROM " +
                        TableName + " WHERE id=@id",
                        new { id = id });
                }
            }
            catch(Exception)
            {
                return new Metric { Id = 0 };
            }
        }

        protected IList<Metric> AbstractGetByTimePeriod(int agentId, long fromTime, long toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {                
                return connection.Query<Metric>("SELECT Id, agentId, Time, Value FROM " +
                    TableName +
                    " WHERE Time >= @fromTime AND Time <= @toTime AND agentId = @agentId", 
                    new
                    {
                        fromTime = fromTime,
                        toTime = toTime,
                        agentId = agentId
                    }).ToList();
            }
        }

        protected IList<Metric> AbstractGetByTimePeriod(long fromTime, long toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<Metric>("SELECT Id, agentId, Time, Value FROM " +
                    TableName +
                    " WHERE Time >= @fromTime AND Time <= @toTime",
                    new
                    {
                        fromTime = fromTime,
                        toTime = toTime
                    }).ToList();
            }
        }

        public TimeSpan GetMaxRegisteredDate(int agentId)
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    /*Dictionary<string, object> queryResult = new Dictionary<string, object>();
                    IList<MaxMetrix> result = connection.Query<MaxMetrix>("SELECT MAX(Time) AS [Max] FROM " +
                        TableName).ToList();
                    MaxMetrix maxValue = result[0];
                    if(maxValue.Max != null)
                        return maxValue.Max;
                    else
                        return DateTime.UtcNow.TimeOfDay;*/
                    var result = connection.ExecuteScalar<long>(
                        $"SELECT MAX(Time) FROM "+
                        TableName +
                        " WHERE agentId = @agentId",
                        new { agentId });
                    return new TimeSpan(result);
                }
            }
            catch(Exception)
            {
                return DateTime.UtcNow.TimeOfDay;
            }
        }
    }
}
