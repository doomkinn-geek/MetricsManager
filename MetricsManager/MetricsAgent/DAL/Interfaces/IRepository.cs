using MetricsAgent.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DAL.Interfaces
{
    public interface IRepository<T>
    {
        IList<T> GetAll();

        T GetById(int id);

        void Create(T item);

        void Update(T item);

        void Delete(int id);
    }

}
