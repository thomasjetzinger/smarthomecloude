using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;
using SmartHomeCloudAPI.Models;

namespace SmartHomeCloudAPI.Data
{


    public class EventDataConnector
    {
        private CloudTable table;


        public EventDataConnector()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);
            // Create the CloudTable object that represents the "events" table.
            table = storageAccount.CreateCloudTableClient().GetTableReference("Events");
        }







        public TableQuerySegment<EventEntity> GetAllEvents(TableContinuationToken token)
        {
            TableQuery<EventEntity> query = new TableQuery<EventEntity>().Take(100);
            TableQuerySegment<EventEntity> resultSegment = table.ExecuteQuerySegmented(query, token);

            return resultSegment;

        }

        public TableQuerySegment<EventEntity> GetEvents(TableContinuationToken token, EventFilter filter)
        {

           var list = (from events in table.CreateQuery<EventEntity>()
                       where events.alerttype.Equals(filter.type) select events
               );

            
            TableQuery<EventEntity> query = new TableQuery<EventEntity>().Where(filter.GetFilterString()).Take(100);
            TableQuerySegment<EventEntity> resultSegment = table.ExecuteQuerySegmented(query, token);

            return resultSegment;

        }


    }
}