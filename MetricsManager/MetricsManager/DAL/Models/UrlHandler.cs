using Dapper;
using System;
using System.Data;
using System.Security.Policy;

namespace MetricsManager.DAL.Repositories
{
    public class UrlHandler : SqlMapper.TypeHandler<Uri>
    {
        public override Uri Parse(object value)
        {
            string value1 = (string)value;
            Uri res = new Uri(value1);            
            return res;
        }

        public override void SetValue(IDbDataParameter parameter, Uri value)
            => parameter.Value = value;
    }

}