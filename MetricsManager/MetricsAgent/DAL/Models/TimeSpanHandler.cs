using Dapper;
using System;
using System.Data;

namespace MetricsAgent.DAL.Repositories
{
    public class TimeSpanHandler : SqlMapper.TypeHandler<TimeSpan>
    {
        public override TimeSpan Parse(object value)
            => TimeSpan.FromSeconds((long)value);

        public override void SetValue(IDbDataParameter parameter, TimeSpan value)
            => parameter.Value = value;
    }

}