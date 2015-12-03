using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Runtime.Serialization;

namespace SmartHomeCloudAPI.Data
{
    [DataContract]
    public class SensorValueEntity : TableEntity
    {
        [DataMember]       
        public string displayname { get; set; }
        [DataMember]
        public string connectiondeviceid { get; set; }
        [DataMember]
        public string guid { get; set; }
        [DataMember]
        public string location { get; set; }
        [DataMember]
        public string measurename { get; set; }
        [DataMember]
        public string organization { get; set; }
        [DataMember]
        public string unitofmeasure { get; set; }
        [DataMember]
        public double value { get; set; }
        [DataMember]
        public DateTimeOffset timestamp {
            get { return Timestamp; }
            set { Timestamp = timestamp; }
        }

    }
}