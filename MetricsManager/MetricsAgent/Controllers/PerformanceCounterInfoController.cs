using Core.PerformanceCountersHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Controllers
{
    [Route("api/info")]
    [ApiController]
    public class PerformanceCounterInfoController : ControllerBase
    {
        [HttpGet("countercategory")]
        public IActionResult InfoByCategory([FromQuery] string categoryName, string counterName)
        {
            List<string> strs = new List<string>();
            PerformanceCounterCategory Category = new PerformanceCounterCategory(categoryName);
            String[] instancename = Category.GetInstanceNames();

            foreach (string name in instancename)
            {
                strs.Add(name);
            }
            return Ok(strs);
            //return Ok(Category.GetInstanceNames());
            //return Ok(PerformanceCounterHelper.GetAllCounters(categoryName));
            return Ok(Category.GetCounters(counterName));            
        }
    }
}
