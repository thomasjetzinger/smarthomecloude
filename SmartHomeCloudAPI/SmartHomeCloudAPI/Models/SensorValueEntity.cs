using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace SmartHomeCloudAPI.Data
{
    public class SensorValueEntity : TableEntity
    {

        public string displayname { get; set; }
        public string connectiondeviceid { get; set; }        
        public string guid { get; set; }
        public string location { get; set; }
        public string measurename { get; set; }
        public string organization { get; set; }
        public DateTime timestamp { get; set; }
        public string unitofmeasure { get; set; }
        public double value { get; set; }
        public DateTime EventProcessedUtcTime { get; set; }
        public DateTime EventEnqueuedUtcTime { get; set; }

    }
}