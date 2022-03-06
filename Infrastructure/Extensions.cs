using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Infrastructure.Queues;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IStorageQueueClient, StorageQueueClient>();
            services.RegisterStorageQueuePublishers();

            return services;
        }

        public static IServiceCollection RegisterStorageQueuePublishers(this IServiceCollection services)
        {
            return services.AddSingleton<IStorageQueueClient>(serviceProvider =>
            {
                var storageQueueClientLogger = serviceProvider.GetRequiredService<ILogger<IStorageQueueClient>>();
                var logger = serviceProvider.GetRequiredService<ILogger<StorageQueueClient>>();
                var webJobConfiguration = serviceProvider.GetRequiredService<IConfiguration>();

                return new StorageQueueClient(storageQueueClientLogger, RegisterStorageQueuesPublishers(webJobConfiguration, logger));
            });
        }

        private static QueueClient RegisterStorageQueuesPublishers(IConfiguration configuration, ILogger<StorageQueueClient> logger)
        {
            QueueClientOptions queueClientOptions = new QueueClientOptions()
            {
                MessageEncoding = QueueMessageEncoding.Base64
            };
            QueueClient queueClient = new QueueClient(configuration["AzureWebJobsStorage"], configuration["UserDetailsQueueName"], queueClientOptions);
            try
            {
                // Try to create a queue that already exists
                queueClient.Create();

            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == QueueErrorCode.QueueAlreadyExists)
            {
                // Ignore any errors if the queue already exists
            }

            return queueClient;
        }
    }
}
