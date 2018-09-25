using System;
using System.Collections.Generic;
using System.Linq;
using AzureTableStorePoC.ConfigurationHandler;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableStorePoc.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var client = new Client("ConnectionStringHere");
            var handler = new Handler(client);
            handler.SetTableReference("Configuration");

            
            // Add single configuration entity.
            var item = handler.AddEntity(
                new ConfigurationEntity("Enoro", "MaxMessageSize", "Prodat", @"200")).GetAwaiter().GetResult();
            

            // Add multiple configuration entities.
            var items = handler.AddEntities(new List<ConfigurationEntity>
            {
                new ConfigurationEntity("Enoro", "ExportFilePath", "Prodat", @"\\path\to\export\files"),
                new ConfigurationEntity("Enoro", "ImportFilePath", "Prodat", @"\\path\to\import\files")

            }).GetAwaiter().GetResult();

            // Get all configuration entities for Enoro.
            var entities = handler.GetConfigurationsByTenant("Enoro").GetAwaiter().GetResult();

            System.Console.WriteLine($"{entities.Count} entities fetched.");
            foreach (var entity in entities)
                System.Console.WriteLine(
                    $"Application: {entity.Application}, Key: {entity.RowKey}, Value: {entity.Value}");
            
            System.Console.ReadKey();
        }
    }
}
