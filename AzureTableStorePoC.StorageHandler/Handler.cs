using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableStorePoC.ConfigurationHandler
{
    public class Handler
    {
        private readonly IClient _client;
        private CloudTable _table;

        public Handler(IClient client)
        {
            _client = client;
        }

        public void SetTableReference(string tableName)
        {
             _table = _client.GetTableReference(tableName);
        }

        public async Task<TableResult> AddEntity(ITableEntity entity)
        {
            var insertOperation = TableOperation.InsertOrMerge(entity);

            return await _table.ExecuteAsync(insertOperation);
        }

        public async Task<IList<TableResult>> AddEntities(IEnumerable<ITableEntity> entities)
        {
            var batchOperation = new TableBatchOperation();

            foreach(var entity in entities)
                batchOperation.InsertOrMerge(entity);

            return await _table.ExecuteBatchAsync(batchOperation);
        }

        public async Task<TableResult> GetSingleEntity(string tenant, string key)
        {
            var retrieveOperation = TableOperation.Retrieve<ConfigurationEntity>(tenant, key);

            return await _table.ExecuteAsync(retrieveOperation);
        }

        public async Task<List<ConfigurationEntity>> GetConfigurationsByTenant(string tenant, CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = new TableQuery<ConfigurationEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, tenant));

            var entities = new List<ConfigurationEntity>();
            TableContinuationToken token = null;
            do
            {
                var resultSegment = await _table.ExecuteQuerySegmentedAsync<ConfigurationEntity>(query, token);
                token = resultSegment.ContinuationToken;
                entities.AddRange(resultSegment);
            } while (token != null && !cancellationToken.IsCancellationRequested);

            return entities;
        }

        public async Task<TableResult> DeleteEntity(string tenant, string key)
        {
            var deleteOperation =
                TableOperation.Delete(new ConfigurationEntity(tenant, key) { ETag = "*" });

            return await _table.ExecuteAsync(deleteOperation);
        }
    }
}
