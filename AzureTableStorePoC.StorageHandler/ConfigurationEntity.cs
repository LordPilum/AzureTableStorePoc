using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableStorePoC.ConfigurationHandler
{
    public class ConfigurationEntity : TableEntity
    {
        public ConfigurationEntity() { }

        public ConfigurationEntity(string tenant, string keyName)
        {
            PartitionKey = tenant;
            RowKey = keyName;
        }

        public ConfigurationEntity(string tenant, string keyName, string application, string value)
        {
            PartitionKey = tenant;
            RowKey = keyName;
            Application = application;
            Value = value;
        }

        public string Application { get; set; }
        public string Value { get; set; }
    }
}
