using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Core.PerformanceCountersHelper
{
    public static class PerformanceCounterHelper
    {
        public static List<string> GetAllCounters(string categoryFilter)
        {
            List<string> res = new List<string>();
            var categories = PerformanceCounterCategory.GetCategories();
            foreach (var cat in categories)
            {
                //res.Add(cat.CategoryName);
                //res.Add(cat.CategoryType.ToString());
                if (categoryFilter != null && categoryFilter.Length > 0)
                {
                    if (!cat.CategoryName.Contains(categoryFilter)) continue;
                }
                res.Add($"Category {cat.CategoryName}");
                try
                {
                    var instances = cat.GetInstanceNames();
                    if (instances != null && instances.Length > 0)
                    {
                        foreach (var instance in instances)
                        {
                            //if (cat.CounterExists(instance))
                            //{
                            foreach (var counter in cat.GetCounters(instance))
                            {
                                res.Add($"\tCounter Name {counter.CounterName} [{instance}]");
                            }
                            //}
                        }
                    }
                    else
                    {
                        foreach (var counter in cat.GetCounters())
                        {
                            res.Add($"\tCounter Name {counter.CounterName}");
                        }
                    }
                }
                catch (Exception)
                {
                    // NO COUNTERS
                }
            }
            return res;
        }
    }
}
