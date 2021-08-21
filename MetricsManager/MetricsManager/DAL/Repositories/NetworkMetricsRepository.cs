﻿using Core;
using Dapper;
using MetricsManager.DAL.Models;
using MetricsManager.DAL.Repositories;
using MetricsManager.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL
{

    public class NetworkMetricsRepository : AbstractRepository, IRepository<Metric>
    {
        public NetworkMetricsRepository(IConfiguration _config) : base("allnetworkmetrics", _config)
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

        public IList<Metric> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            return AbstractGetByTimePeriod(fromTime, toTime);
        }
    }

}
