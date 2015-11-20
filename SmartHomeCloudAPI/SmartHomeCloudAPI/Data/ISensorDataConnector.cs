using System.Collections.Generic;

namespace SmartHomeCloudAPI.Data
{
    public interface ISensorDataConnector
    {
        IEnumerable<SensorValueEntity> GetAllEntries();
    }
}