using Core;
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
    public class AgentsRepository : IRepository<AgentItem>
    {
        private string TableName { get; set; }
        private IConfiguration Configuration { get; set; }
        private string ConnectionString { get; set; }

        public AgentsRepository(IConfiguration _configuration)
        {
            TableName = "agents";
            this.Configuration = _configuration;
            ConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
            SqlMapper.AddTypeHandler(new UrlHandler());
        }
        public void Create(AgentItem item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {                
                connection.Execute("INSERT INTO " +
                    TableName +
                    " (agentUrl) VALUES(@agentUrl)",                    
                    new
                    {                        
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

        public IList<AgentItem> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                // читаем при помощи Query и в шаблон подставляем тип данных
                // объект которого Dapper сам и заполнит его поля
                // в соответсвии с названиями колонок
                return connection.Query<AgentItem>("SELECT Id, agentUrl FROM " + TableName).ToList();
            }
        }

        public AgentItem GetById(int id)
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    return connection.QuerySingle<AgentItem>("SELECT Id, agentUrl FROM " +
                        TableName + " WHERE id=@id",
                        new { id = id });
                }
            }
            catch (Exception)
            {
                return new AgentItem { Id = 0 };
            }
        }

        public void Update(AgentItem item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE " + TableName + " SET Id = @Id, agentUrl = @value WHERE id=@id", new
                {
                    Id = item.Id,                    
                    agentUrl = item.AgentUrl
                });
            }
        }
    }
}
