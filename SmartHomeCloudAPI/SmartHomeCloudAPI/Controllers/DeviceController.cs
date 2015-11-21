using Microsoft.Azure.Devices;
using SmartHomeCloudAPI.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SmartHomeCloudAPI.Controllers
{
    public class DeviceController : ApiController
    {
        private const int MAX_COUNT_OF_DEVICES = 1000;
        private static readonly string protocolGatewayHost = String.Empty;
        private static IOTHubDeviceExplorer iotHubExplorer;

        static DeviceController()
        {
            iotHubExplorer = new IOTHubDeviceExplorer(
               ConfigurationManager.AppSettings["IotHubConnectionString"], MAX_COUNT_OF_DEVICES, protocolGatewayHost);


        }


        public async Task<IEnumerable<DeviceEntity>> GetDeviceList()
        {
            var devicesList = await iotHubExplorer.GetDevices();

            return devicesList;
        }

        [HttpGet]
        [Route("api/Device/{id}")]
        public async Task<DeviceEntity> GetDevice(string id)
        {
            var devicesList = await iotHubExplorer.GetDevices();

            return devicesList.FirstOrDefault(device => device.Id.Equals(id)); ;
        }

        [HttpPost]
        [Route("api/Device/{id}")]
        public async Task<string> PostCommand([FromUri]string id, [FromBody]string command)
        {

            if (!string.IsNullOrEmpty(command))
                await iotHubExplorer.SendCloudToDeviceMessageAsync(id, command);
            return command;
        }

    }
}
