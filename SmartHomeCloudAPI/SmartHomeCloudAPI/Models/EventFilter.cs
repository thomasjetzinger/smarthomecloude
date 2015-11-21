using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartHomeCloudAPI.Models
{

    public class EventFilter
    {
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public String type { get; set; }


        public string GetFilterString()
        {
            List<string> filters = new List<string>();

            if (type != null)
            {
                filters.Add(TableQuery.GenerateFilterCondition("alerttype", QueryComparisons.Equal, type));

            }

            if (from != null)
            {
                filters.Add(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, from.Value));
            }

            if (to != null)
            {
                filters.Add(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.LessThanOrEqual, to.Value));
            }

            //create filters
            if (filters.Count == 0)
            {
                return "";
            }
            else if (filters.Count == 1)
            {
                return filters.First();
            }
            else
            {
                String finalFilter = TableQuery.CombineFilters(filters[0], TableOperators.And, filters[1]);
                for (int i = 2; i < filters.Count; i++)
                {
                    finalFilter = TableQuery.CombineFilters(finalFilter, TableOperators.And, filters[i]);
                }
                return finalFilter;
            }
        }
    }
}