using Core;
using Dapper;
using MetricsManager.DAL.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Repositories
{
    public class AgentsRepository : IRepository<AgentMetric>
    {
        private string TableName { get; set; }
        private IConfiguration Configuration { get; set; }
        private string ConnectionString { get; set; }

        public AgentsRepository(IConfiguration _configuration)
        {
            TableName = "agents";
            this.Configuration = _configuration;
            ConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
        }
        public void Create(AgentMetric item)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {                
                connection.Execute("INSERT INTO " +
                    TableName +
                    " (Id, agentId, agentUrl) VALUES(@Id, @agentId, @agentUrl)",                    
                    new
                    {
                        Id = item.Id,
                        agentId = item.AgentId,
                        agentUrl = item.AgentUrl
                    });
            }
        }

        public void Delete(int id)
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

        public IList<AgentMetric> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                // читаем при помощи Query и в шаблон подставляем тип данных
                // объект которого Dapper сам и заполнит его поля
                // в соответсвии с названиями колонок
                return connection.Query<AgentMetric>("SELECT Id, agentId, agentUrl FROM " + TableName).ToList();
            }
        }

        public AgentMetric GetById(int id)
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    return connection.QuerySingle<AgentMetric>("SELECT Id, agentId, agentUrl FROM " +
                        TableName + " WHERE id=@id",
                        new { id = id });
                }
            }
            catch (Exception e)
            {
                return new AgentMetric { Id = 0 };
            }
        }

        public void Update(AgentMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE " + TableName + " SET Id = @Id, agentId = @agentId, agentUrl = @value WHERE id=@id", new
                {
                    Id = item.Id,
                    agentId = item.AgentId,
                    agentUrl = item.AgentUrl
                });
            }
        }
    }
}
