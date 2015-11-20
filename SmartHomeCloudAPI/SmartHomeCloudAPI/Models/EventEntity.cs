using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartHomeCloudAPI.Models
{
    public class EventEntity : TableEntity
    {   
        public string alerttype { get; set; }
        public string displayname { get; set; }
        public string guid { get; set; }
        public string location { get; set; }
        public string measurename { get; set; }
        public string message { get; set; }
        public string organization { get; set; }
        public double tempmax { get; set; }
        public DateTime timecreated { get; set; }
        public string unitofmeasure { get; set; }
        public double value { get; set; }
    }
}