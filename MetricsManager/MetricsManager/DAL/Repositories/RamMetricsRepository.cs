using Core;
using Dapper;
using MetricsManager.DAL.Models;
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

        public IList<Metric> GetByTimePeriod(int agentId, long fromTime, long toTime)
        {
            return AbstractGetByTimePeriod(agentId, fromTime, toTime);
        }
    }

}
