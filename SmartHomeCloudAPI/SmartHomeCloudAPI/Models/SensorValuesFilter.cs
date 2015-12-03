using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartHomeCloudAPI.Models
{

    public class SensorValueFilter
    {
        public DateTime? dateFrom { get; set; }
        public DateTime? dateTo { get; set; }
        public string displayname { get; set; }
        public string location { get; set; }
        public string guid { get; set; }
        public string connectiondeviceid { get; set; }        
        public string measurename { get; set; }
        public string organization { get; set; }
        public long? valueFrom { get; set; }
        public long? valueTo { get; set; }
        public string unitofmeasure { get; set; }


        public string GetFilterString()
        {
            List<string> filters = new List<string>();

            if (displayname != null)
            {
                filters.Add(TableQuery.GenerateFilterCondition("displayname", QueryComparisons.Equal, displayname));
            }
            if (location != null)
            {
                filters.Add(TableQuery.GenerateFilterCondition("location", QueryComparisons.Equal, location));
            }
            if (measurename != null)
            {
                filters.Add(TableQuery.GenerateFilterCondition("measurename", QueryComparisons.Equal, measurename));
            }
            if (organization != null)
            {
                filters.Add(TableQuery.GenerateFilterCondition("organization", QueryComparisons.Equal, organization));
            }
            if (guid != null)
            {
                filters.Add(TableQuery.GenerateFilterCondition("guid", QueryComparisons.Equal, guid));
            }
            if (connectiondeviceid != null)
            {
                filters.Add(TableQuery.GenerateFilterCondition("connectiondeviceid", QueryComparisons.Equal, connectiondeviceid));
            }
            if (unitofmeasure != null)
            {
                filters.Add(TableQuery.GenerateFilterCondition("unitofmeasure", QueryComparisons.Equal, unitofmeasure));
            }

            if (valueFrom.HasValue)
            {
                filters.Add(TableQuery.GenerateFilterConditionForLong("value", QueryComparisons.GreaterThanOrEqual, valueFrom.Value));
            }

            if (valueTo.HasValue)
            {
                filters.Add(TableQuery.GenerateFilterConditionForLong("value", QueryComparisons.LessThanOrEqual, valueTo.Value));
            }


            if (dateFrom.HasValue)
            {
                filters.Add(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, dateFrom.Value));
            }

            if (dateTo.HasValue)
            {
                filters.Add(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.LessThanOrEqual, dateTo.Value));
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