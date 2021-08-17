using Core;
using Dapper;
using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Repositories;
using MetricsManager.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace MetricsManager.DAL
{

    public class RamMetricsRepository : AbstractRepository, IRepository<Metric>
    {        
        public RamMetricsRepository(IConfiguration _config) : base("allrammetrics", _config)
        {
            // добавляем парсилку типа TimeSpan в качестве подсказки для SQLite
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }
        public void Create(Metric item)
        {
            AbstractCreate(item);
        }

        public void Delete(int id)
        {
            AbstractDelete(id);
        }

        public void Update(Metric item)
        {
            AbstractUpdate(item);
        }

        public IList<Metric> GetAll()
        {
            return AbstractGetAll();
        }

        public Metric GetById(int id)
        {
            return AbstractGetById(id);
        }

        public IList<Metric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            return AbstractGetByTimePeriod(fromTime, toTime);
        }
    }

}
