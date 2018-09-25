using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableStorePoC.ConfigurationHandler
{
    public class Client : IClient
    {
        private readonly CloudTableClient _tableClient;

        public Client(string azureConnectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(azureConnectionString);
            _tableClient = storageAccount.CreateCloudTableClient();
        }

        public CloudTable GetTableReference(string table)
        {
            var tableReference = _tableClient.GetTableReference(table);

            var success = tableReference.CreateIfNotExistsAsync().GetAwaiter().GetResult();

            return tableReference;
        }
    }

    public interface IClient
    {
        CloudTable GetTableReference(string table);
    }
}
