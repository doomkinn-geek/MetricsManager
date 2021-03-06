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

    public class HddMetricsRepository : AbstractRepository, IRepository<Metric>
    {
        public HddMetricsRepository() : base("hddmetrics")
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
