using Core;
using Dapper;
using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DAL
{

    public class DotNetMetricsRepository : IRepository<MetricContainer>
    {
        private const string tableName = "dotnetmetrics";
        public DotNetMetricsRepository()
        {
            // добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }
        public void Create(MetricContainer item)
        {
            AbstractRepository.AbstractCreate(item, tableName);
        }

        public void Delete(int id)
        {
            AbstractRepository.AbstractDelete(id, tableName);
        }

        public void Update(MetricContainer item)
        {
            AbstractRepository.AbstractUpdate(item, tableName);
        }

        public IList<MetricContainer> GetAll()
        {
            return AbstractRepository.AbstractGetAll(tableName);
        }

        public MetricContainer GetById(int id)
        {
            return AbstractRepository.AbstractGetById(id, tableName);
        }

        public IList<MetricContainer> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            return AbstractRepository.AbstractGetByTimePeriod(fromTime, toTime, tableName);
        }
    }

}
